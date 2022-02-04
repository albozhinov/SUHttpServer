using System.IO;
using System.Linq;

namespace SUHttpServer.HTTP
{
    public class ViewResponse : ContentResponse
    {
        private const char PathSeparator = '/';


        public ViewResponse(string viewName, string controllerName, object model = null)
            : base("", ContentType.Html)
        {
            if (!viewName.Contains(PathSeparator))
            {
                viewName = controllerName + PathSeparator + viewName;
            }
            var viewPath = Path.GetFullPath(
                $"./Views/" 
                + viewName.TrimStart(PathSeparator) 
                + ".cshtml");

            var viewContent = File.ReadAllText(viewPath);

            if (model != null)
            {
                viewContent = PopulateModel(viewContent, model);
            }
            
            Body = viewContent;
        }

        private string PopulateModel(string viewContent, object model)
        {
            var data = model
                        .GetType()
                        .GetProperties()
                        .Select(p => new
                        {
                            p.Name,
                            Value = p.GetValue(model)
                        });

            foreach (var entry in data)
            {
                const string openingBrackets = "{{";
                const string closingBrackets = "}}";

                viewContent = viewContent.Replace($"{openingBrackets}{entry.Name}{closingBrackets}", entry.Value.ToString());
            }

            return viewContent;
        }
    }
}
