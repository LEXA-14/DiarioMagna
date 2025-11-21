SELECT * FROM AspNetRoles
SELECT * FROM AspNetUsers

Select *from AspNetUserRoles


DECLARE @SysUserId NVARCHAR(450);
DECLARE @SysRoleId NVARCHAR(450);

SELECT @SysUserId = 'aca28830-f926-4849-8f4b-288e7678c90d';
SELECT @SysRoleId = '7df92a1d-afa3-4226-9166-a6710ecfb738';

IF NOT EXISTS (
    SELECT 1
    FROM AspNetUserRoles
    WHERE UserId = @SysUserId AND RoleId = @SysRoleId
)
BEGIN
    INSERT INTO AspNetUserRoles (UserId, RoleId)
    VALUES (@SysUserId, @SysRoleId);
END;

SELECT 'Rol asignado correctamente' AS Resultado;