using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Anonymize.Benchmark
{
    public class RecursiveReplace : IReplace
    {
        public string ReplaceJsonForbiddenVariables(string jsonStr)
        {
            if (string.IsNullOrWhiteSpace(jsonStr) || jsonStr == "\"\"")
            {
                return jsonStr;
            }

            try
            {
                var deserializedRequest = JsonConvert.DeserializeObject(jsonStr);
                if (deserializedRequest is JArray jArray)
                {
                    foreach (var jItem in jArray)
                    {
                        ReplaceJsonForbiddenTags(jItem as JObject);
                    }

                    return JsonConvert.SerializeObject(jArray, Formatting.Indented);
                }

                ReplaceJsonForbiddenTags(deserializedRequest as JObject);
                return JsonConvert.SerializeObject(deserializedRequest, Formatting.Indented);
            }
            catch (Exception ex)
            {
                return jsonStr;
            }
        }
        
        /// <summary>
        /// Заменяем запрещенные тэги
        /// </summary>
        /// <param name="jObject">JObject с данными</param>
        private void ReplaceJsonForbiddenTags(JObject jObject)
        {
            if (jObject == null)
            {
                return;
            }

            foreach (KeyValuePair<string, JToken?> item in jObject)
            {
                switch (item.Value)
                {
                    case null:
                        continue;
                    case JObject nestedObject:
                        ReplaceJsonForbiddenTags(nestedObject);
                        break;
                    case JArray jArray:
                    {
                        foreach (var jItem in jArray)
                        {
                            ReplaceJsonForbiddenTags(jItem as JObject);
                        }

                        break;
                    }

                    default:
                    {
                        if (Program.AnonymizePattern.Params.Any(paramName =>
                            item.Key.Equals(paramName, StringComparison.OrdinalIgnoreCase)))
                        {
                            if (item.Value.ToString() == "Chocolate")
                            {
                                jObject[item.Key] = "BINGO!";
                            }
                            else
                            {
                                jObject[item.Key] = "************";
                            }
                        }
                        
                        break;
                    }
                }
            }
        }
    }
}