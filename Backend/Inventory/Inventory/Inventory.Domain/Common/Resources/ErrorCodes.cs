using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Domain.Common.Resources
{
    public static class ErrorCodes
    {
        private static readonly string ErrorCodesFilePath = "D:\\Learning\\BoughtIt\\Backend\\Inventory\\Inventory\\Inventory.Domain\\Common\\Resources\\ErrorCodes.json";
        private static Dictionary<string, ErrorDetail> _errorCodes;

        static ErrorCodes()
        {
            LoadErrorCodes();
        }

        private static void LoadErrorCodes()
        {
            if (File.Exists(ErrorCodesFilePath))
            {
                var jsonContent = File.ReadAllText(ErrorCodesFilePath);
                _errorCodes = JsonSerializer.Deserialize<Dictionary<string, ErrorDetail>>(jsonContent);
            }
            else
            {
                _errorCodes = new Dictionary<string, ErrorDetail>();
            }
        }

        public static ErrorDetail GetError(string key)
        {
            if (_errorCodes.TryGetValue(key, out var errorDetail))
            {
                return errorDetail;
            }

            return null;
        }
    }
}
