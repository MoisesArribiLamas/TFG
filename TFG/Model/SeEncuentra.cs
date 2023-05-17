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
    
    public partial class SeEncuentra
    {
        public long seEncuentraId { get; set; }
        public System.TimeSpan horaIni { get; set; }
        public System.TimeSpan horaFin { get; set; }
        public System.DateTime fecha { get; set; }
        public long bateriaId { get; set; }
        public long estadoId { get; set; }
    
        
        /// <summary>
        /// Relationship Name (Foreign Key in ER-Model): FK_BATERIA_SEENCUENTRA
        /// </summary>
        public virtual Bateria Bateria { get; set; }
        
        /// <summary>
        /// Relationship Name (Foreign Key in ER-Model): FK_ESTADO_SEENCUENTRA
        /// </summary>
        public virtual Estado Estado { get; set; }
    
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
    
    			hash = hash * multiplier + horaIni.GetHashCode();
    			hash = hash * multiplier + horaFin.GetHashCode();
    			hash = hash * multiplier + fecha.GetHashCode();
    			hash = hash * multiplier + bateriaId.GetHashCode();
    			hash = hash * multiplier + estadoId.GetHashCode();
    
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
    	    
            SeEncuentra target = obj as SeEncuentra;
    
    		return true
               &&  (this.seEncuentraId == target.seEncuentraId )       
               &&  (this.horaIni == target.horaIni )       
               &&  (this.horaFin == target.horaFin )       
               &&  (this.fecha == target.fecha )       
               &&  (this.bateriaId == target.bateriaId )       
               &&  (this.estadoId == target.estadoId )       
               ;
    
        }
    
    
    	public static bool operator ==(SeEncuentra  objA, SeEncuentra  objB)
        {
            // Check if the objets are the same SeEncuentra entity
            if(Object.ReferenceEquals(objA, objB))
                return true;
      
            return objA.Equals(objB);
    }
    
    
    	public static bool operator !=(SeEncuentra  objA, SeEncuentra  objB)
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
    	    StringBuilder strSeEncuentra = new StringBuilder();
    
    		strSeEncuentra.Append("[ ");
           strSeEncuentra.Append(" seEncuentraId = " + seEncuentraId + " | " );       
           strSeEncuentra.Append(" horaIni = " + horaIni + " | " );       
           strSeEncuentra.Append(" horaFin = " + horaFin + " | " );       
           strSeEncuentra.Append(" fecha = " + fecha + " | " );       
           strSeEncuentra.Append(" bateriaId = " + bateriaId + " | " );       
           strSeEncuentra.Append(" estadoId = " + estadoId + " | " );       
            strSeEncuentra.Append("] ");    
    
    		return strSeEncuentra.ToString();
        }
    
    
    }
}
