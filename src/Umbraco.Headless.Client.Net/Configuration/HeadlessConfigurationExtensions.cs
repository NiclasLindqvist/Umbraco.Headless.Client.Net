using System.Linq;
using Newtonsoft.Json;
using Umbraco.Headless.Client.Net.Delivery;
using Umbraco.Headless.Client.Net.Delivery.Models;
using Umbraco.Headless.Client.Net.Serialization;

namespace Umbraco.Headless.Client.Net.Configuration
{
    internal static class HeadlessConfigurationExtensions
    {
        public static JsonConverter[] GetJsonConverters(this IHeadlessConfiguration configuration,
            ModelNameResolver modelNameResolver)
        {
            if (configuration is IStronglyTypedHeadlessConfiguration stronglyTypedHeadlessConfiguration)
            {
                return new JsonConverter[]
                {
                    new ModelConverter<IContent, Content>(
                        stronglyTypedHeadlessConfiguration.ContentModelTypes.ToDictionary(modelNameResolver.GetContentModelAlias),
                        "contentTypeAlias"
                    ),
                    new ModelConverter<IMedia, Media>(
                        stronglyTypedHeadlessConfiguration.MediaModelTypes.ToDictionary(modelNameResolver.GetMediaModelAlias),
                        "mediaTypeAlias"
                    ),
                    new ModelConverter<IElement, Element>(
                        stronglyTypedHeadlessConfiguration.ElementModelTypes.ToDictionary(modelNameResolver.GetElementModelAlias),
                        "contentTypeAlias"
                    ),
                };
            }

            return new JsonConverter[0];
        }
    }
}
