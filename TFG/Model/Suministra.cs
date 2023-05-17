//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Es.Udc.DotNet.PracticaMaD.Model
{
    using System;
    using System.Text;
    using System.Collections.Generic;
    
    public partial class Suministra
    {
        public long suministraId { get; set; }
        public long bateriaId { get; set; }
        public long tarifaId { get; set; }
        public System.TimeSpan horaIni { get; set; }
        public System.TimeSpan horaFin { get; set; }
        public double kws { get; set; }
        public double ahorro { get; set; }
    
        
        /// <summary>
        /// Relationship Name (Foreign Key in ER-Model): FK_BATERIA_SUMINISTRA
        /// </summary>
        public virtual Bateria Bateria { get; set; }
        
        /// <summary>
        /// Relationship Name (Foreign Key in ER-Model): FK_TARIFA_SUMINISTRA
        /// </summary>
        public virtual Tarifa Tarifa { get; set; }
    
    	/// <summary>
    	/// A hash code for this instance, suitable for use in hashing algorithms and data structures 
    	/// like a hash table. It uses the Josh Bloch implementation from "Effective Java"
        /// Primary key of entity is not included in the hash calculation to avoid errors
    	/// with Entity Framework creation of key values.
    	/// </summary>
    	/// <returns>
    	/// Returns a hash code for this instance.
    	/// </returns>
    	public override int GetHashCode()
    	{
    	    unchecked
    	    {
    			int multiplier = 31;
    			int hash = GetType().GetHashCode();
    
    			hash = hash * multiplier + bateriaId.GetHashCode();
    			hash = hash * multiplier + tarifaId.GetHashCode();
    			hash = hash * multiplier + horaIni.GetHashCode();
    			hash = hash * multiplier + horaFin.GetHashCode();
    			hash = hash * multiplier + kws.GetHashCode();
    			hash = hash * multiplier + ahorro.GetHashCode();
    
    			return hash;
    	    }
    
    	}
        
        /// <summary>
        /// Compare this object against another instance using a value approach (field-by-field) 
        /// </summary>
        /// <remarks>See http://www.loganfranken.com/blog/687/overriding-equals-in-c-part-1/ for detailed info </remarks>
    	public override bool Equals(object obj)
    	{
    
            if (ReferenceEquals(null, obj)) return false;        // Is Null?
            if (ReferenceEquals(this, obj)) return true;         // Is same object?
            if (obj.GetType() != this.GetType()) return false;   // Is same type?
    	    
            Suministra target = obj as Suministra;
    
    		return true
               &&  (this.suministraId == target.suministraId )       
               &&  (this.bateriaId == target.bateriaId )       
               &&  (this.tarifaId == target.tarifaId )       
               &&  (this.horaIni == target.horaIni )       
               &&  (this.horaFin == target.horaFin )       
               &&  (this.kws == target.kws )       
               &&  (this.ahorro == target.ahorro )       
               ;
    
        }
    
    
    	public static bool operator ==(Suministra  objA, Suministra  objB)
        {
            // Check if the objets are the same Suministra entity
            if(Object.ReferenceEquals(objA, objB))
                return true;
      
            return objA.Equals(objB);
    }
    
    
    	public static bool operator !=(Suministra  objA, Suministra  objB)
        {
            return !(objA == objB);
        }
    
    
        /// <summary>
        /// Returns a <see cref="T:System.String"></see> that represents the 
        /// current <see cref="T:System.Object"></see>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"></see> that represents the current 
        /// <see cref="T:System.Object"></see>.
        /// </returns>
    	public override String ToString()
    	{
    	    StringBuilder strSuministra = new StringBuilder();
    
    		strSuministra.Append("[ ");
           strSuministra.Append(" suministraId = " + suministraId + " | " );       
           strSuministra.Append(" bateriaId = " + bateriaId + " | " );       
           strSuministra.Append(" tarifaId = " + tarifaId + " | " );       
           strSuministra.Append(" horaIni = " + horaIni + " | " );       
           strSuministra.Append(" horaFin = " + horaFin + " | " );       
           strSuministra.Append(" kws = " + kws + " | " );       
           strSuministra.Append(" ahorro = " + ahorro + " | " );       
            strSuministra.Append("] ");    
    
    		return strSuministra.ToString();
        }
    
    
    }
}