using System.Text.RegularExpressions;

namespace AmvReporting.Infrastructure.Helpers
{
    public static class RavenIdResolver
    {
        public static int ResolveRavenId(this string ravenId)
        {
            var match = Regex.Match(ravenId, @"\d+");
            var idStr = match.Value;
            int id = int.Parse(idStr);
            if (id == 0)
            {
                throw new System.InvalidOperationException("Id cannot be zero.");
            }

            return id;
        }
    }
}