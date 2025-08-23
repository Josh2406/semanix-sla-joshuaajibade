namespace FormsService.Application.Extensions
{
    public static class StringExtensions
    {
        public static bool IsValidJsonPayload(object json)
        {
            try
            {
                var jsonString = json.ToString();
                var payload = JsonSerializer.Deserialize<JsonPayloadDto>(jsonString!, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                if (payload != null && payload.Sections != null && payload.Sections.Count > 0)
                {
                    var fieldTypes = new List<string> { "text", "number", "date", "dropdown", "checkbox" };
                    foreach (var section in payload.Sections)
                    {
                        var invalidFields = section.Fields
                            .Where(f => string.IsNullOrEmpty(f.FieldId) || string.IsNullOrEmpty(f.Label)
                                || string.IsNullOrEmpty(f.Type) || !fieldTypes.Contains(f.Type.ToLower()))
                            .ToList();

                        if (invalidFields != null && invalidFields.Count > 0)
                        {
                            return false;
                        }

                        var orders = section.Fields.Select(f => f.Order).ToList();
                        if (orders.Count != orders.Distinct().Count())
                        {
                            return false;
                        }
                    }
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }

            return true;
        }
    }
}
