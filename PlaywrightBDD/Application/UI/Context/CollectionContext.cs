using Application.UI.Pages;
using Framework.Assertions;
using Framework.Core;
using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Application.UI.Context
{
    /// <summary>
    /// Generic interaction with PocketBase collections grid.
    /// Works with any collection by using column headers and object properties.
    /// </summary>
    public sealed class CollectionContext
    {
        private readonly IPage _pwPage;
        private readonly CollectionPage _page;

        public CollectionContext(IPage page, ElementExecutor executor)
        {
            _pwPage = page;
            _page   = new CollectionPage(page, executor);
        }

        /// <summary>
        /// Opens a collection by name and waits for its grid to load.
        /// Uses the sidebar via AppShell instead of hardcoded routes.
        /// </summary>
        public async Task OpenAsync(string collectionName)
        {
            // Use the generic sidebar menu when possible
            await _page.Shell.Menu.OpenCollectionAsync(collectionName);
            await _page.Grid.WaitForLoadedAsync();
        }

        /// <summary>
        /// Creates a new record in the given collection using an object as source.
        /// Property names are mapped to modal field labels (case-insensitive).
        ///
        /// keyColumn is the grid column that uniquely identifies the row (e.g. "email").
        /// keySelector extracts the key value from the object, for post-create validation.
        /// </summary>
        public async Task CreateRecordFromObjectAsync<T>(
            string collectionName,
            T data,
            string keyColumn,
            Func<T, string> keySelector)
            where T : class, new()
        {
            // 1. Open the collection
            await OpenAsync(collectionName);

            // 2. Open the "create" modal via toolbar
            await _page.Shell.Toolbar.ClickCreateAsync();

            // 3. Wait for modal to be visible
            await _page.Modal.WaitForOpenAsync(10000);

            // 4. Map object -> field labels/values
            var fieldMap = ObjectToFieldMap(data);

            foreach (var kv in fieldMap)
            {
                var label = kv.Key;
                var value = kv.Value ?? string.Empty;

                // This assumes modal labels ≈ property names (case-insensitive),
                // which matches your users collection: email, Password, etc.
                await _page.Modal.FillFieldAsync(label, value);
            }

            // 5. Confirm and wait for grid reload
            await _page.Modal.ConfirmAsync();
            await _page.Grid.WaitForLoadedAsync();

            // 6. Optional: verify that the created row matches the object
            var key = keySelector(data);
            await AssertRowMatchesAsync(keyColumn, key, data);
        }

        /// <summary>
        /// Reads a single row (identified by keyColumn/keyValue) and maps it into an object T
        /// by matching public property names to grid column headers.
        /// </summary>
        public async Task<T> GetRowAsObjectAsync<T>(
            string keyColumn,
            string keyValue)
            where T : new()
        {
            var rowLocator = await _page.Grid.FindRowByCellEqualsAsync(keyColumn, keyValue);
            var cells      = await _page.Grid.ReadRowAsDictionaryAsync(rowLocator);

            return MapCellsToObject<T>(cells);
        }

        /// <summary>
        /// Asserts that a single row in the grid matches the given object, field by field,
        /// for any property that has a matching column in the grid.
        /// </summary>
        public async Task AssertRowMatchesAsync<T>(
            string keyColumn,
            string keyValue,
            T expected)
            where T : new()
        {
            var actual = await GetRowAsObjectAsync<T>(keyColumn, keyValue);
            AssertObjectMatches(expected, actual, $"{keyColumn}={keyValue}");
        }

        /// <summary>
        /// Asserts that all the given expected objects exist in the grid and match field by field.
        /// keyColumn is the grid column used to uniquely identify rows (e.g. "email").
        /// keySelector extracts the key value from the object.
        /// </summary>
        public async Task AssertRowsMatchAsync<T>(
            string keyColumn,
            IEnumerable<T> expectedItems,
            Func<T, string> keySelector)
            where T : new()
        {
            var list = expectedItems.ToList();
            GenericAssert.CollectionNotEmpty(list, "Expected items collection is empty.");

            foreach (var expected in list)
            {
                var key = keySelector(expected);
                await AssertRowMatchesAsync(keyColumn, key, expected);
            }
        }

        // ------------------------
        // Helpers
        // ------------------------

        /// <summary>
        /// Maps object properties into a dictionary of "field label" -> "text value".
        /// By default uses property name as label (case-insensitive match).
        /// 
        /// This is where your model/builder logic can plug in later:
        /// you can replace this logic with attributes or custom mapping 
        /// between properties and labels.
        /// </summary>
        private static IReadOnlyDictionary<string, string?> ObjectToFieldMap<T>(T obj)
            where T : class
        {
            var result = new Dictionary<string, string?>(StringComparer.OrdinalIgnoreCase);

            var props = typeof(T)
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(p => p.CanRead);

            foreach (var prop in props)
            {
                var name  = prop.Name; // default label = property name
                var value = prop.GetValue(obj);

                // For now, we just ToString() everything (UI is all text anyway).
                var text = value?.ToString();

                if (text != null)
                {
                    result[name] = text;
                }
            }

            return result;
        }

        private static T MapCellsToObject<T>(IReadOnlyDictionary<string, string> cells)
            where T : new()
        {
            var obj = new T();
            var props = typeof(T)
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(p => p.CanWrite && p.CanRead);

            foreach (var prop in props)
            {
                var name = prop.Name;

                // look for matching column
                var cell = cells.FirstOrDefault(kv =>
                    string.Equals(kv.Key, name, StringComparison.OrdinalIgnoreCase));

                if (cell.Key == null)
                    continue;

                var cellValue = cell.Value ?? string.Empty;
                object? converted = cellValue;

                if (prop.PropertyType != typeof(string))
                {
                    try
                    {
                        if (string.IsNullOrWhiteSpace(cellValue))
                        {
                            converted = null;
                        }
                        else
                        {
                            converted = Convert.ChangeType(cellValue, prop.PropertyType);
                        }
                    }
                    catch
                    {
                        converted = cellValue;
                    }
                }

                prop.SetValue(obj, converted);
            }

            return obj;
        }

        private static void AssertObjectMatches<T>(T expected, T actual, string? context)
        {
            var prefix = string.IsNullOrWhiteSpace(context) ? "" : $"[{context}] ";

            var props = typeof(T)
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(p => p.CanRead);

            foreach (var prop in props)
            {
                var expVal  = prop.GetValue(expected);
                var actVal  = prop.GetValue(actual);

                var expText = expVal?.ToString() ?? string.Empty;
                var actText = actVal?.ToString() ?? string.Empty;

                GenericAssert.IsEqual(
                    actText,
                    expText,
                    $"{prefix}Property '{prop.Name}' mismatch. Expected '{expText}', got '{actText}'.");
            }
        }

        /// <summary>
        /// Expose the underlying page mapping in case a test or flow
        /// needs direct access to grid, modal, or shell.
        /// </summary>
        public CollectionPage Page => _page;
    }
}