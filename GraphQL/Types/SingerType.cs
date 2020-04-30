// using GraphQL.Authorization;
// using GraphQL.Types;
// using Leo.Services.Muses.Entities;

// namespace FullStackJobs.GraphQL.Infrastructure.GraphQL.Types
// {
//     public class SingerType : ObjectGraphType<Singer>
//     {
//         public SingerType(ContextServiceLocator contextServiceLocator)
//         {
//             Field(x => x.Id);
//             Field(x => x.Position);
//             Field(x => x.Company, true);
//             Field(x => x.Icon);
//             Field(x => x.Location, true);
//             Field(x => x.AnnualBaseSalary, true).AuthorizeWith(Policies.Employer);
//             Field(x => x.Description, true);
//             Field(x => x.Responsibilities, true);
//             Field(x => x.Requirements, true);
//             Field(x => x.ApplicationInstructions, true);
//             Field<ListGraphType<TagType>>("tags", resolve: context => context.Source.Tags);
//             Field<EnumerationGraphType<Status>>("status");
//             Field<StringGraphType>("modified", resolve: context => contextServiceLocator.Humanizer.TimeAgo(context.Source.Modified ?? context.Source.Created));
//             Field<IntGraphType>("songCount", resolve: context => context.Source.JobApplicants.Count);
//         }
//     }
// }