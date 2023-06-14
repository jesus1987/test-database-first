

 --PREGUNTA 1
 
 SELECT  total = Sum(vd.TotalLinea), cantidad = sum(vd.Cantidad)
FROM [Prueba].[dbo].[Venta] v
JOIN [Prueba].[dbo].[VentaDetalle] vd on v.ID_Venta = vd.ID_Venta
WHERE v.Fecha >=dateadd(dd,-30,getdate())

--PREGUNTA 2
SELECT v.*
FROM [Prueba].[dbo].[Venta] v
WHERE v.Total = (
SELECT MAX(v.Total)
FROM [Prueba].[dbo].[Venta] v
WHERE v.Fecha >=dateadd(dd,-30,getdate()))

--PREGUNTA 3
SELECT TOP 1 p.Nombre, total = Sum(vd.TotalLinea)
FROM [Prueba].[dbo].[Venta] v
JOIN [Prueba].[dbo].[VentaDetalle] vd on v.ID_Venta = vd.ID_Venta
JOIN [Prueba].[dbo].[Producto] p on p.ID_Producto = vd.ID_Producto
WHERE v.Fecha >=dateadd(dd,-30,getdate())
GROUP BY p.Nombre
ORDER BY total DESC

--PREGUNTA 4
SELECT TOP 1 l.Nombre, total = Sum(vd.TotalLinea)
FROM [Prueba].[dbo].[Venta] v
JOIN [Prueba].[dbo].[VentaDetalle] vd on v.ID_Venta = vd.ID_Venta
JOIN [Prueba].[dbo].[Local] l on l.ID_Local = v.ID_Local
WHERE v.Fecha >=dateadd(dd,-30,getdate())
GROUP BY l.Nombre
ORDER BY total DESC
--PREGUNTA 5
SELECT TOP 1 Marca = m.Nombre, 
       MargenGanancia = SUM((vd.Precio_Unitario - p.Costo_Unitario) * vd.Cantidad) 
FROM [Prueba].[dbo].[Marca] m
JOIN [Prueba].[dbo].[Producto] p ON m.ID_Marca = p.ID_Marca
JOIN [Prueba].[dbo].[VentaDetalle] vd ON p.ID_Producto = vd.ID_Producto
JOIN [Prueba].[dbo].[Venta] v on v.ID_Venta = vd.ID_Venta
WHERE v.Fecha >=dateadd(dd,-30,getdate())
GROUP BY M.Nombre
ORDER BY MargenGanancia DESC


-- PREGUNTA 6
;with temp as (
	SELECT v.ID_Local, vd.ID_Producto, MasVendido = SUM(vd.Cantidad)
	FROM [Prueba].[dbo].[Venta] v
	JOIN [Prueba].[dbo].[VentaDetalle] vd on vd.ID_Venta = v.ID_Venta
	WHERE v.Fecha >=dateadd(dd,-30,getdate())
	GROUP BY v.ID_Local, vd.ID_Producto
)
SELECT t.ID_Local, t.ID_Producto, t.MasVendido
FROM(select *, MasVendidoTemp=MAX(MasVendido) OVER (partition by ID_Local)
		from temp) t
WHERE t.MasVendido = t.MasVendidoTemp



