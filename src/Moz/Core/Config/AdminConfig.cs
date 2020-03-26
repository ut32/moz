namespace Moz.Core.Config
{
    public class AdminConfig
    {
        public string Title { get; set; } = "MOZ后台管理系统";
        public string Path { get; set; } = "admin";
        public string LoginView { get; set; }
        public string WelcomeView { get; set; }
    }
}