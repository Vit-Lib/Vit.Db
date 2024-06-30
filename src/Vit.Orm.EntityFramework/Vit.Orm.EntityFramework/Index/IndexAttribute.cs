using System;

namespace Vit.Orm.EntityFramework.Index
{


    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
    public class IndexAttribute : System.Attribute
    {
        /// <summary>
        /// name of index. can be null
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool IsUnique { get; set; } = false;


        public IndexAttribute(string Name = null, bool IsUnique = false)
        {
            this.Name = Name;
            this.IsUnique = IsUnique;
        }
    }
}
