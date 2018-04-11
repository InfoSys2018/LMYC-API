using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace LmycWeb.ViewModels
{
    public class LogoutViewModel
    {
        [BindNever]
        public string RequestId { get; set; }
    }
}
