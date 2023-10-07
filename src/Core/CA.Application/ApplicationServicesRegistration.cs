
using CA.Application.DTOs.Ent.TValue;
using CA.Application.DTOs.Generic;
using CA.Application.Features.Generic.Commands;
using CA.Application.Features.Generic.Queries;
using CA.Domain.Base;
using CA.Domain.Ent;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace CA.Application
{
    public static class ApplicationServicesRegistration
    {
        public static IServiceCollection ConfigureApplicationServices(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddMediatR(Assembly.GetExecutingAssembly());
            AddTransientForProfiels<TValue, TValueDto>(services);



            return services;
        }
        private static void AddTransientForProfiels<BE, BT>(IServiceCollection services)
            where BE : BaseEntity
            where BT : BaseDto
        {
            services.AddTransient<IRequestHandler<GetListBaseQuery<BT, BE>, List<BT>>, GetListBaseQueryHandler<BT, BE>>();
            services.AddTransient<IRequestHandler<GetListByFopFilterQuery<BT, BE>, (List<BT>, int)>, GetListByFopFilterQueryHandler<BT, BE>>();
            services.AddTransient<IRequestHandler<GetDetailBaseQuery<BT, BE>, BT>, GetDetailBaseQueryHandler<BT, BE>>();
            services.AddTransient<IRequestHandler<CreateBaseCommand<BE>, int>, CreateBaseCommandHandler<BE>>();
            services.AddTransient<IRequestHandler<UpdateBaseCommand<BE>, Unit>, UpdateBaseCommandHandler<BE>>();
            services.AddTransient<IRequestHandler<DeleteBaseCommand<BE>, Unit>, DeleteBaseCommandHandler<BE>>();
        }
    }

    public delegate Type ConverterDelegate();
    public interface IMediatorServiceTypeConverter
    {
        Type Convert(Type sourceType, ConverterDelegate next);
    }
    public static class MediatorServiceFactory
    {
        public static ServiceFactory Wrap(ServiceFactory serviceFactory,
            IEnumerable<IMediatorServiceTypeConverter> converters)
        {
            return serviceType =>
            {
                Type NoConversion() => serviceType;
                var convertServiceType = converters
                    .Reverse()
                    .Aggregate((ConverterDelegate)NoConversion, (next, c) => () => c.Convert(serviceType, next));
                return serviceFactory(convertServiceType());
            };
        }
        public class TestConverter : IMediatorServiceTypeConverter
        {
            public Type Convert(Type sourceType, ConverterDelegate next)
            {
                var isRequestHandler = sourceType.IsGenericType &&
                                       sourceType.GetGenericTypeDefinition() == typeof(IRequestHandler<,>);
                if (!isRequestHandler) return next();

                var requestType0 = sourceType.GenericTypeArguments[0];
                var shouldConvertType = requestType0.IsGenericType &&
                                            requestType0.GetGenericTypeDefinition() == typeof(GetListBaseQuery<,>);
                if (!shouldConvertType) return next();
                var returnType0 = requestType0.GenericTypeArguments[0];

                return typeof(GetListBaseQueryHandler<,>).MakeGenericType(returnType0).MakeGenericType();
            }
        }
    }
}
