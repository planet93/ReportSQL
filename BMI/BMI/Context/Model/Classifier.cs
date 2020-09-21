using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BMI.Context.Model
{
    public class Classifier
    {
        /// <inheritdoc />
        /// <summary>
        /// Идентификатор
        /// </summary>
        [Key]
        public virtual long Id { get; set; }
        /// <inheritdoc />
        /// <summary>
        /// Дата занесения записи
        /// </summary>
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        /// <summary>
        /// Дата модификации записи
        /// </summary>
        public DateTime UpdatedOn { get; set; } = DateTime.Now;
        /// <inheritdoc />
        /// <summary>
        /// Активен
        /// </summary>
        [DefaultValue(true)]
        public bool Active { get; set; } = true;
        /// <inheritdoc />
        /// <summary>
        /// Удален
        /// </summary>
        [DefaultValue(false)]
        public bool Deleted { get; set; }
        /// <summary>
		/// Наименование
		/// </summary>
		[Index("Index_Classifier_Name", 1, IsUnique = false)]
        [MinLength(1)]
        [MaxLength(250)]
        public string Name { get; set; }
        /// <summary>
        /// Наименование
        /// </summary>
        [Index("Index_Classifier_Code", 1, IsUnique = false)]
        [MinLength(1)]
        [MaxLength(250)]
        public string Code { get; set; }
        public double Value { get; set; }
        /// <summary>
        /// Тип справочника
        /// </summary>
        public virtual ClassifierType ClassifierType { get; set; }
        /// <summary>
        /// Родитель
        /// </summary>
        public virtual Classifier Parent { get; set; }
        /// <summary>
        /// Порядок сортировки
        /// </summary>
        public int SortOrder { get; set; }
        //public DateTime DateFrom { get; set; }
        //public DateTime DateTo { get; set; }
        //public long Year { get; set; }
        //public long Month { get; set; }
        public string Description { get; set; }
    }
}