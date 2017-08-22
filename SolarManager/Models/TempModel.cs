using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace SolarManager.Models
{
    public enum TempStatus { Active = 0, Inactive, Suspended };
    public class TempModel
    {
        [Key]
        public int Id { get; set; }

        [StringLength(50, MinimumLength = 5)]
        public string Name { get; set; }
        [DisplayFormat(DataFormatString = "{0:C}")]
        public decimal Value { get; set; }
        public int TypeId { get; set; }
        public TempType Type { get; set; }

        public static List<TempModel> GetList()
        {
            List<TempModel> retval = new List<TempModel>();
            TempModel temp_model;
            Random rand = new Random(DateTime.Now.Millisecond);

            for (int i = 0; i < 20; i++)
            {
                decimal value = Convert.ToDecimal(rand.NextDouble() * 100);
                int type_id = rand.Next(1, 13);
                temp_model = new TempModel { Id = i, Name = $"Temp Item {i}", Value = value, TypeId = type_id, Type = TempType.GetList().FirstOrDefault(t => t.Id == type_id) };
                retval.Add(temp_model);
            }

            return retval;
        }
    }

    public class TempType
    {
        [Key]
        public int Id { get; set; }
        public string Description { get; set; }
        public TempStatus Status { get; set; }

        public static List<TempType> GetList()
        {
            List<TempType> retval = new List<TempType>();
            TempType temp_type = new TempType { Id = 1, Description = "Type One", Status = TempStatus.Active };
            retval.Add(temp_type);
            temp_type = new TempType { Id = 2, Description = "Type Two", Status = TempStatus.Inactive };
            retval.Add(temp_type);
            temp_type = new TempType { Id = 3, Description = "Type Three", Status = TempStatus.Active };
            retval.Add(temp_type);
            temp_type = new TempType { Id = 4, Description = "Type Four", Status = TempStatus.Active };
            retval.Add(temp_type);
            temp_type = new TempType { Id = 5, Description = "Type Five", Status = TempStatus.Active };
            retval.Add(temp_type);
            temp_type = new TempType { Id = 6, Description = "Type Six", Status = TempStatus.Inactive };
            retval.Add(temp_type);
            temp_type = new TempType { Id = 7, Description = "Type Seven", Status = TempStatus.Active };
            retval.Add(temp_type);
            temp_type = new TempType { Id = 8, Description = "Type Eight", Status = TempStatus.Active };
            retval.Add(temp_type);
            temp_type = new TempType { Id = 9, Description = "Type Nine", Status = TempStatus.Inactive };
            retval.Add(temp_type);
            temp_type = new TempType { Id = 10, Description = "Type Ten", Status = TempStatus.Inactive };
            retval.Add(temp_type);
            temp_type = new TempType { Id = 11, Description = "Type Eleven", Status = TempStatus.Suspended };
            retval.Add(temp_type);
            temp_type = new TempType { Id = 12, Description = "Type Twelve", Status = TempStatus.Suspended };
            retval.Add(temp_type);

            return retval;
        }
    }
}