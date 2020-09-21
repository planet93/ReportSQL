using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BMI.Context.Model
{
    public class ClassifierType
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
        [Index("Index_ClassifierType_Name", 1, IsUnique = true)]
        [MinLength(1)]
        [MaxLength(50)]
        public string Name { get; set; }
        public string RusName { get; set; }

        /// <summary>
        /// Является системным типом
        /// </summary>
        public bool SystemType { get; set; } = false;
    }
}