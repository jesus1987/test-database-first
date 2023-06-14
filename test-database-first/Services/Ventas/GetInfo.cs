using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Globalization;
using test_database_first.Models;
using test_database_first.Repositories;

namespace test_database_first.Services.Ventas
{
    public class GetInfo: IRequest<IEnumerable<string>>
    {
    }

    internal class HandlerGetInfo : IRequestHandler<GetInfo, IEnumerable<string>>
    {
        private readonly IRepository<Local> localRepository;
        private readonly IRepository<Marca> marcaRepository;
        private readonly IRepository<Producto> productoRepository;
        private readonly IRepository<VentaDetalle> ventaDetalleRepository;
        private readonly IRepository<Venta> ventaRepository;
        public HandlerGetInfo(IRepository<Local> localRepository, IRepository<Marca> marcaRepository,
            IRepository<Producto> productoRepository, IRepository<VentaDetalle> ventaDetalleRepository, IRepository<Venta> ventaRepository)
        {
            this.ventaDetalleRepository = ventaDetalleRepository;
            this.localRepository = localRepository;
            this.marcaRepository = marcaRepository;
            this.productoRepository = productoRepository;
            this.ventaRepository = ventaRepository;
        }

        public async Task<IEnumerable<string>> Handle(GetInfo request, CancellationToken cancellationToken)
        {
            var startDate = DateTime.Now.Date.AddDays(-30);
            var result = new List<string>();

            var queryVentaDetallada = (from v in ventaRepository.Query
                        join vd in ventaDetalleRepository.Query on v.IdVenta equals vd.IdVenta
                        join p in productoRepository.Query on vd.IdProducto equals p.IdProducto
                        join l in localRepository.Query on v.IdLocal equals l.IdLocal
                        join m in marcaRepository.Query on p.IdMarca equals m.IdMarca
                        where v.Fecha.Date >= startDate.Date
                        select new 
                        { 
                            v.IdVenta, 
                            v.Fecha, 
                            v.IdLocal,
                            VentaTotal = v.Total,
                            vd.Cantidad,
                            vd.TotalLinea,
                            vd.PrecioUnitario,
                            p.IdProducto,
                            NombreProducto = p.Nombre,
                            CodigoProducto = p.Codigo,
                            ModeloProducto = p.Modelo,
                            CostoUnitarioProducto = p.CostoUnitario,
                            NombreLocal = l.Nombre,

                        }).ToList();

            result.Add($"PREG 1==>Monto Total:{queryVentaDetallada.Sum(at=> at.TotalLinea).ToString("C", CultureInfo.CurrentCulture)}, Cantidad Total Ventas: {queryVentaDetallada.Sum(at=>at.Cantidad)}");

            var max = queryVentaDetallada.Max(at => at.VentaTotal);
            var preg2= (from v in queryVentaDetallada
                       where v.VentaTotal == max
                       select new { Date = v.Fecha, v.VentaTotal, v.IdVenta}).Distinct();
            foreach (var item in preg2)
            {
                result.Add($"PREG 2 ==> Fecha y Hora: {item.Date}, Monto:{item.VentaTotal.ToString("C", CultureInfo.CurrentCulture)}, ID:{item.IdVenta}");
            }

            var preg3 = from v in queryVentaDetallada
                        group v by v.NombreProducto into newGroup
                        
                        select new { newGroup.Key, VentaTotal = newGroup.Sum(at => at.TotalLinea) };

            var productVenta = preg3.OrderByDescending(at=>at.VentaTotal).FirstOrDefault();


            result.Add($"PREG 3 ==> Producto: {productVenta?.Key}, Monto:{productVenta?.VentaTotal.ToString("C", CultureInfo.CurrentCulture)}");

            var preg4 = from v in queryVentaDetallada
                        group v by v.NombreLocal into newGroup

                        select new { newGroup.Key, VentaTotal = newGroup.Sum(at => at.TotalLinea) };

            var local = preg4.OrderByDescending(at => at.VentaTotal).FirstOrDefault();
            result.Add($"PREG 4 ==> Local: {local?.Key}, Monto:{local?.VentaTotal.ToString("C", CultureInfo.CurrentCulture)}");

            // pregunta 5 y 6 no esta usando el mismo query newcesita agruparce 
            var preg5 = ( from p in productoRepository.Query
                                        join md in marcaRepository.Query on p.IdMarca equals md.IdMarca
                                        join vd in ventaDetalleRepository.Query on p.IdProducto equals vd.IdProducto
                                        join v in ventaRepository.Query on vd.IdVenta equals v.IdVenta
                                        where v.Fecha.Date >= startDate.Date
                                        group new { p, vd, v } by md into g
                                        select new
                                        {
                                            MargenGanancia = g.Sum(x => (x.vd.PrecioUnitario - x.p.CostoUnitario) * x.vd.Cantidad),
                                            Marca = g.Key.Nombre
                                        }
                                    ).OrderByDescending(at=>at.MargenGanancia).FirstOrDefault();
            result.Add($"PREG 5 ==> Marca: {preg5?.Marca}, MargenGanancia:{preg5?.MargenGanancia.ToString("C", CultureInfo.CurrentCulture)}");

            var preg6 = (
                from l in localRepository.Query
                join v in ventaRepository.Query on l.IdLocal equals v.IdLocal
                join vd in ventaDetalleRepository.Query on v.IdVenta equals vd.IdVenta
                join p in productoRepository.Query on vd.IdProducto equals p.IdProducto
                where v.Fecha.Date >= startDate.Date
                group new { l, p } by l into g
                select new
                {
                    ProductoMasVendido = (g.OrderByDescending(x => x.p.IdProducto).FirstOrDefault()).p.Nombre,
                    Local = g.Key.Nombre
                }
            ).ToList();

            foreach (var item in preg6)
            {
                result.Add($"PREG 6 ==> Local: {item.Local}, Producto Mas Vendido:{item.ProductoMasVendido}");
            }
            return await Task.FromResult(result);
        }
    }
}
