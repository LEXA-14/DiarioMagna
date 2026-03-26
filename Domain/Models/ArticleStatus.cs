using System.ComponentModel.DataAnnotations;

namespace DiarioMagna.Domain.Models;

public enum ArticleStatus
{
    
    Borrador = 0,
  
    Pendiente = 1,
    
    Aprobado = 2,
   
    Rechazado = 3,
    
    Enviado = 4
}
