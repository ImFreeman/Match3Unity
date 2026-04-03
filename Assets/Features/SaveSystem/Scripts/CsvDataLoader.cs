using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using UnityEngine;
using UnityEngine.Networking;

namespace Assets.Features.SaveSystem.Scripts
{
    public static class CsvDataLoader
    {
        public static async UniTask<List<RecordData>> LoadFromStreamingAssetsAsync(string fileName, CancellationToken token = default)
        {
            var records = new List<RecordData>();
            string filePath = System.IO.Path.Combine(Application.streamingAssetsPath, fileName);

            if (filePath.StartsWith("jar") || filePath.StartsWith("http"))
            {
                using (UnityWebRequest request = UnityWebRequest.Get(filePath))
                {
                    await request.SendWebRequest().ToUniTask(cancellationToken: token);

                    if (request.result == UnityWebRequest.Result.Success)
                    {
                        ParseCsvContent(request.downloadHandler.text, records);
                    }
                    else
                    {
                        Debug.LogError($"Failed to load file: {request.error}");
                    }
                }
            }
            else
            {
                try
                {
                    string fileContent = await File.ReadAllTextAsync(filePath, token);
                    ParseCsvContent(fileContent, records);
                }
                catch (OperationCanceledException)
                {
                    Debug.Log($"Loading cancelled for: {filePath}");
                    throw;
                }
                catch (Exception ex)
                {
                    Debug.LogError($"Error reading file {filePath}: {ex.Message}");
                }
            }
            

            return records;
        }

        private static void ParseCsvContent(string content, List<RecordData> records)
        {
            var lines = content.Split('\n');
            bool isFirstLine = true;

            foreach (var line in lines)
            {
                if (isFirstLine) { isFirstLine = false; continue; }
                if (string.IsNullOrWhiteSpace(line)) continue;

                var parts = line.Trim().Split(',');
                if (parts.Length < 2) continue;

                if (DateTime.TryParse(parts[0], out DateTime date) && int.TryParse(parts[1], out int value))
                {
                    records.Add(new RecordData { Date = date, Value = value });
                }
            }
        }
    }
}
