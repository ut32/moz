namespace System
{
    public static class UriExtensions
    {
        public static string GetBaseDomain(this Uri uri)
        {
            if (uri == null) return null;
            if (uri.HostNameType != UriHostNameType.Dns) return null;

            var domain = uri.DnsSafeHost;
            if (string.IsNullOrEmpty(domain)) return null;

            var ary = domain.Split('.');
            switch (ary.Length)
            {
                case 2:
                    return domain;
                case 3:
                    return $"{ary[1]}.{ary[2]}";
                default:
                    return null;
            }
        }
    }
}