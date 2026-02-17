using Application.API.Dtos;
using Application.Shared.Models;
using System;
using System.Threading.Tasks;

namespace Application.API
{
    public class PocketBaseApi
    {
        private readonly ApiClient _api;

        public PocketBaseApi(string baseUrl)
        {
            _api = new ApiClient(baseUrl);
        }

        // ===============================
        // AUTH
        // ===============================

        public async Task AdminLoginAsync(string identity, string password)
        {
            var endpoints = new[]
            {
                "api/collections/_superusers/auth-with-password",
                "api/admins/auth-with-password"
            };

            foreach (var ep in endpoints)
            {
                var res = await _api.PostAsync<AdminAuthResponse>(ep, new { identity, password });

                if ((int)res.StatusCode == 404)
                    continue;

                if (!res.IsSuccess || res.Data == null || string.IsNullOrWhiteSpace(res.Data.Token))
                    throw new Exception($"Admin login failed at '{ep}'. Status={res.StatusCode}, Body={res.Body}");

                _api.SetBearerToken(res.Data.Token);
                return;
            }

            throw new Exception("Admin login failed: no known admin auth endpoint matched (404).");
        }

        // ===============================
        // GENERIC RECORD OPERATIONS
        // ===============================

        public async Task<PbRecord> CreateRecordAsync(string collection, PbRecord record)
        {
            var res = await _api.PostAsync<PbRecordResponse>(
                $"api/collections/{collection}/records",
                record.Fields
            );

            if (!res.IsSuccess || res.Data == null)
                throw new Exception(
                    $"CreateRecord failed. Status={res.StatusCode}, Body={res.Body}"
                );

            return MapToPbRecord(res.Data);
        }

        public async Task<PbRecord> UpdateRecordAsync(string collection, string id, PbRecord record)
        {
            var res = await _api.PatchAsync<PbRecordResponse>(
                $"api/collections/{collection}/records/{id}",
                record.Fields
            );

            if (!res.IsSuccess || res.Data == null)
                throw new Exception(
                    $"UpdateRecord failed. Status={res.StatusCode}, Body={res.Body}"
                );

            return MapToPbRecord(res.Data);
        }

        public async Task<PbRecord> GetRecordAsync(string collection, string id)
        {
            var res = await _api.GetAsync(
                $"api/collections/{collection}/records/{id}"
            );

            if (!res.IsSuccess)
                throw new Exception(
                    $"GetRecord failed. Status={res.StatusCode}, Body={res.Body}"
                );

            var dto = System.Text.Json.JsonSerializer.Deserialize<PbRecordResponse>(
                res.Body,
                new System.Text.Json.JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

            if (dto == null)
                throw new Exception("Failed to deserialize record response.");

            return MapToPbRecord(dto);
        }

        public async Task DeleteRecordAsync(string collection, string id)
        {
            var res = await _api.DeleteAsync(
                $"api/collections/{collection}/records/{id}"
            );

            if (!res.IsSuccess)
                throw new Exception(
                    $"DeleteRecord failed. Status={res.StatusCode}, Body={res.Body}"
                );
        }

        public async Task<ApiResponse> HealthAsync()
        {
            return await _api.GetAsync("api/health");
        }


        // ===============================
        // INTERNAL MAPPING
        // ===============================

        private static PbRecord MapToPbRecord(PbRecordResponse dto)
        {
            var record = new PbRecord
            {
                Id = dto.Id
            };

            foreach (var kv in dto.Fields)
            {
                record.Set(kv.Key, kv.Value);
            }

            return record;
        }
    }
}