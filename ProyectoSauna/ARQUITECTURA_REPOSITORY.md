# Arquitectura Repository Pattern - Sistema Sauna

## Resumen
Se ha implementado el patrón Repository junto con servicios de aplicación para separar la lógica de negocio del acceso a datos, específicamente para el manejo del login/autenticación.

## Estructura Implementada

### 1. DTOs (Data Transfer Objects)
- **LoginRequestDTO**: Contiene los datos de entrada para el login
- **LoginResultDTO**: Contiene el resultado del proceso de autenticación

### 2. Interfaces
- **IAuthService**: Define los métodos de autenticación y manejo de contraseñas
- **IUsuarioRepository**: Define métodos específicos para operaciones con usuarios

### 3. Servicios
- **AuthService**: Implementa la lógica de negocio de autenticación
- **PasswordHelper**: Utilidad para hash y verificación de contraseñas

### 4. Beneficios de la Implementación

#### Separación de Responsabilidades
- **LoginSauna.xaml.cs**: Solo maneja la interfaz de usuario
- **AuthService**: Maneja toda la lógica de autenticación
- **UsuarioRepository**: Maneja el acceso a datos de usuarios

#### Testabilidad
- Cada componente puede ser probado de forma independiente
- Se pueden crear mocks de las interfaces para pruebas unitarias

#### Mantenibilidad
- Cambios en la lógica de autenticación no afectan la UI
- Cambios en el acceso a datos no afectan la lógica de negocio

#### Reutilización
- El AuthService puede ser usado desde cualquier parte de la aplicación
- Los métodos de hash de contraseñas están centralizados

## Flujo de Autenticación

```
1. Usuario ingresa credenciales en LoginSauna.xaml
2. LoginSauna.xaml.cs llama a AuthService.ValidarLoginAsync()
3. AuthService valida los datos de entrada
4. AuthService usa UsuarioRepository para buscar el usuario
5. AuthService verifica la contraseña usando PasswordHelper
6. AuthService retorna LoginResultDTO con el resultado
7. LoginSauna.xaml.cs procesa el resultado y navega o muestra error
```

## Métodos Disponibles en AuthService

### ValidarLoginAsync
- Valida credenciales de usuario
- Retorna información completa del resultado de autenticación

### HashPassword
- Genera hash seguro de contraseñas
- Utiliza SHA256 con salt fijo

### CambiarClaveAsync
- Permite cambio de contraseña validando la contraseña actual
- Ideal para cambios iniciados por el usuario

### RestablecerClaveAsync
- Permite restablecer contraseña sin validar la actual
- Ideal para restablecimientos administrativos

## Registro de Dependencias

Los servicios se registran en `App.xaml.cs` usando el método de extensión `AddApplicationServices()` que está en `ServiceCollectionExtensions.cs`.

## Próximos Pasos Recomendados

1. **Implementar servicios similares para otras entidades** (Clientes, Productos, etc.)
2. **Añadir logging** para auditoría de intentos de login
3. **Implementar políticas de contraseñas** (complejidad, expiración)
4. **Añadir autenticación de dos factores**
5. **Implementar caché** para sesiones de usuario
6. **Añadir pruebas unitarias** para todos los servicios

## Ejemplo de Uso

```csharp
// En cualquier parte de la aplicación
var authService = App.AppHost.Services.GetRequiredService<IAuthService>();

var result = await authService.ValidarLoginAsync("usuario", "contraseña");
if (result.IsSuccess)
{
    // Login exitoso, usar result.NombreUsuario, result.Rol, etc.
}
else
{
    // Mostrar result.ErrorMessage
}
```

Esta implementación establece una base sólida para el crecimiento y mantenimiento del sistema.