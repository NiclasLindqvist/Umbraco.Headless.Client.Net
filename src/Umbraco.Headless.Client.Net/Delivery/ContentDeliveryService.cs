﻿using System;
using System.Net.Http;
using Umbraco.Headless.Client.Net.Collections;
using Umbraco.Headless.Client.Net.Configuration;
using Umbraco.Headless.Client.Net.Delivery.Models;
using Umbraco.Headless.Client.Net.Security;

namespace Umbraco.Headless.Client.Net.Delivery
{
    /// <summary>
    /// Service class for interacting with the Content Delivery API
    /// </summary>
    public class ContentDeliveryService
    {
        /// <summary>
        /// Initializes a new instance of the ContentDeliveryService class
        /// </summary>
        /// <param name="projectAlias">Alias of the Project</param>
        public ContentDeliveryService(string projectAlias) : this(new HeadlessConfiguration(projectAlias))
        { }

        /// <summary>
        /// Initializes a new instance of the ContentDeliveryService class
        /// </summary>
        /// <param name="projectAlias">Alias of the Project</param>
        /// <param name="apiKey">Api Key</param>
        public ContentDeliveryService(string projectAlias, string apiKey) : this(new ApiKeyBasedConfiguration(projectAlias, apiKey))
        { }

        /// <summary>
        /// Initializes a new instance of the ContentDeliveryService class
        /// </summary>
        /// <param name="projectAlias">Alias of the Project</param>
        /// <param name="username">Umbraco Backoffice Username</param>
        /// <param name="password">Umbraco Backoffice User Password</param>
        public ContentDeliveryService(string projectAlias, string username, string password) : this(new PasswordBasedConfiguration(projectAlias, username, password))
        { }

        /// <summary>
        /// Initializes a new instance of the ContentDeliveryService class
        /// </summary>
        /// <param name="configuration">Reference to the <see cref="IHeadlessConfiguration"/></param>
        public ContentDeliveryService(IHeadlessConfiguration configuration) : this (configuration, new HttpClient { BaseAddress = new Uri(Constants.Urls.BaseCdnUrl) })
        { }

        /// <summary>
        /// Initializes a new instance of the ContentDeliveryService class
        /// </summary>
        /// <param name="configuration">Reference to the <see cref="IPasswordBasedConfiguration"/></param>
        public ContentDeliveryService(IPasswordBasedConfiguration configuration)
        {
            var httpClient = new HttpClient(new AuthenticatedHttpClientHandler(configuration))
            {
                BaseAddress = new Uri(Constants.Urls.BaseCdnUrl)
            };

            var modelNameResolver = new ModelNameResolver();

            Content = new ContentDelivery(configuration, httpClient, modelNameResolver);
            Media = new MediaDelivery(configuration, httpClient, modelNameResolver);
        }

        /// <summary>
        /// Initializes a new instance of the ContentDeliveryService class
        /// </summary>
        /// <param name="configuration">Reference to the <see cref="IApiKeyBasedConfiguration"/></param>
        public ContentDeliveryService(IApiKeyBasedConfiguration configuration)
        {
            var httpClient = new HttpClient
            {
                BaseAddress = new Uri(Constants.Urls.BaseCdnUrl),
                DefaultRequestHeaders = { { Constants.Headers.ApiKey, configuration.Token } }
            };

            var modelNameResolver = new ModelNameResolver();

            Content = new ContentDelivery(configuration, httpClient, modelNameResolver);
            Media = new MediaDelivery(configuration, httpClient, modelNameResolver);
        }

        /// <summary>
        /// Initializes a new instance of the ContentDeliveryService class
        /// </summary>
        /// <remarks>
        /// When passing in your own HttpClient you are responsible for setting the authentication headers
        /// </remarks>
        /// <param name="configuration">Reference to the <see cref="IHeadlessConfiguration"/></param>
        /// <param name="httpClient">Reference to the <see cref="HttpClient"/></param>
        public ContentDeliveryService(IHeadlessConfiguration configuration, HttpClient httpClient)
        {
            var modelNameResolver = new ModelNameResolver();

            Content = new ContentDelivery(configuration, httpClient, modelNameResolver);
            Media = new MediaDelivery(configuration, httpClient, modelNameResolver);
        }

        /// <summary>
        /// Gets the Content part of the Content Delivery API
        /// </summary>
        public IContentDelivery Content { get; }

        /// <summary>
        /// Gets the Media part of the Content Delivery API
        /// </summary>
        public IMediaDelivery Media { get; }
    }
}
