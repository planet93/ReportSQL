using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BMI.Context.Model
{
    public class Estimate
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
        public Classifier Month { get; set; }
        public Classifier Spend1 { get; set; }
        public Classifier Spend2 { get; set; }
        public Classifier Spend3 { get; set; }
        public Classifier Spend4 { get; set; }
        public string ActivityName { get; set; }
        public decimal EstimSum { get; set; }
        public decimal TargetSum { get; set; }
    }
}