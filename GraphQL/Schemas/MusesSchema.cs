// using System;
// using GraphQL;
// using GraphQL.Types;
// using Leo.Services.Muses.GraphQL.Mutations;
// using Leo.Services.Muses.GraphQL.Queries;

// namespace Leo.Services.Muses.GraphQL.Schemas
// {

//     public class MusesSchema : Schema
//     {
//         // public InventorySchema(InventoryQuery query, InventoryMutation mutation)
//         // {
//         //     Query = query;
//         //     Mutation = mutation;
//         // }
//         public MusesSchema(IServiceProvider services): base(services)
//         {
//             // Query = resolver.Resolve<InventoryQuery>();
//             Query = services.GetService(typeof(MusesQuery)).As<IObjectGraphType>();
//             Mutation = services.GetService(typeof(MusesMutation)).As<IObjectGraphType>();
//         }
//     }
// }