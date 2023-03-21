using Microsoft.AspNetCore.Cors.Infrastructure;
using ArtBiathlon.DAL.Interfaces;
using ArtBiathlon.DAL.Repositories;
using ArtBiathlon.Services.Implementations;
using ArtBiathlon.Services.Interfaces;
using ArtBiathlon.DataEntity;


namespace ArtBiathlon.Models
{
    public static class Initializer
    {
        public static void InitializeRepositories(this IServiceCollection services)
        {
            services.AddScoped<IBaseRepository<User>, UserRepository>();
            services.AddScoped<IBaseRepository<ForumMessage>, ForumMessageRepository>();
            services.AddScoped<IBaseRepository<ForumReaction>, ForumReactionRepository>();
            services.AddScoped<IBaseRepository<Help>, HelpRepository>();
            services.AddScoped<IBaseRepository<Mailing>, MailingRepository>();
            services.AddScoped<IBaseRepository<MailingTopic>, MailingTopicRepository>();
            services.AddScoped<IBaseRepository<MailingTopicSubscriber>, MailingTopicSubscriberRepository>();
            //services.AddScoped<IBaseRepository<Indicator>, IndicatorRepository>();
            //services.AddScoped<IBaseRepository<Training>, TrainingRepository>();
            //services.AddScoped<IBaseRepository<TrainingTemplate>, TrainingTemplateRepository>();
        }

        public static void InitializeServices(this IServiceCollection services)
        {
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IAdminService, AdminService>();
            services.AddScoped<IPersonalAccountService, PersonalAccountService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IHelpService, HelpService>();
            services.AddScoped<IMailSenderService, MailSenderService>();
            services.AddScoped<IMailingService, MailingService>();
            services.AddScoped<IMailingTopicService, MailingTopicService>();
            services.AddScoped<IMailingTopicSubscriberService, MailingTopicSubscriberService>();
        }
    }
}
