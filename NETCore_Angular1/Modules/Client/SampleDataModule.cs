﻿using Nancy.ModelBinding;
using Newtonsoft.Json;
using NETCore_Angular1.DTO.Criteria.Client;
using NETCore_Angular1.Managers.Client;

namespace NETCore_Angular1.Modules.Client
{
    public sealed class SampleDataModule : BaseModule
    {
        public SampleDataModule(SampleDataManager manager) : base("/client")
        {
            Get("/counter", args => new { count = manager.ClientList.Count });

            Get("/", arg => JsonConvert.SerializeObject(manager.ClientList));

            Post("/", args =>
            {
                var entity = this.Bind<ClientCriteria>();
                entity.Id = manager.ClientList.Count;
                return new { Id = manager.ClientList.Count };
            });
        }
    }
}