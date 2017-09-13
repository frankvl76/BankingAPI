using AutoMapper;
using Banking.Data.Models;
using Banking.DTO.Transactions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Banking.API.Configurations.Automapper
{
    /// <summary>
    /// Class holding all automapper mapping profiles
    /// </summary>
    public class MappingProfile : Profile
    {
        private DateTime _lastMonthsMaxDate;

        /// <summary>
        /// Automapper mapping profile constructor
        /// </summary>
        public MappingProfile()
        {
            // Calculate last months max days date
            if (DateTime.Now.Day > 25)
                _lastMonthsMaxDate = new DateTime(DateTime.Now.Year,
                                         DateTime.Now.Month,
                                         25);
            else
                _lastMonthsMaxDate = new DateTime(DateTime.Now.AddMonths(-1).Year,
                             DateTime.Now.AddMonths(-1).Month,
                             25);

            CreateMap<FixedTransactionCreateDTO, FixedTransaction>();
            CreateMap<TransactionCreateDTO, Transaction>();
            CreateMap<Transaction, TransactionCreateDTO>();
            CreateMap<Transaction, TransactionOverviewDTO>()
                .ForMember(dest => dest.TransactionDate, opts => opts.MapFrom(source => source.TransactionDate_2.ToString("dd-MM-yyyy")))
                .ForMember(dest => dest.Paid, opts => opts.MapFrom(source => IsPaid(source.TransactionDate_1)));
        }

        private bool IsPaid(DateTime transactionDate)
        {
            return transactionDate > _lastMonthsMaxDate;
        }
    }
}
