using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASC.Utilities
{
    public class CurrentUser
    {
        public string Name { get; set; } = string.Empty; // ✅ Gán giá trị mặc định
        public string Email { get; set; } = string.Empty; 
        public bool IsActive { get; set; } = true; // ✅ Mặc định là `true`
        public string[] Roles { get; set; } = Array.Empty<string>(); // ✅ Khởi tạo mảng rỗng
    }
}
