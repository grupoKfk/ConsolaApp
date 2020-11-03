using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;



namespace ConsoleApp1
{
    public class Product
    {
        /// <summary>
        /// Primary Key
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Nombre del Ramo
        /// </summary>
        public string Nombre { get; set; }
        /// <summary>
        /// Fk. RamoProducto
        /// </summary>
        public int RamoProductoId { get; set; }
        /// <summary>
        /// Descrición del producto
        /// </summary>
        public string Descripcion { get; set; }
        /// <summary>
        /// Identifica si se suma impuesto en la factura
        /// </summary>
        public bool AplicaImpuesto { get; set; } = true;
        /// <summary>
        /// Precio de venta sin impuesto aplicado
        /// </summary>
        public decimal PrecioVenta { get; set; }
        /// <summary>
        /// Precio neto con el que compro el producto
        /// </summary>
        public decimal PrecioCosto { get; set; }
        /// <summary>
        /// Código del producto
        /// </summary>
        public string Codigo { get; set; }
        /// <summary>
        /// Estado del producto
        /// </summary>
        public int Estado { get; set; }
    }

    class Program
    {
        static HttpClient client = new HttpClient();
        static void Main(string[] args)
        {
            RunAsync();
        }

        static void ShowProduct(Product product)
        {
            Console.WriteLine($"Name: {product.Nombre}\tPrice Cot: " +
               $"{product.PrecioCosto}\tPrecio Venta: {product.PrecioVenta}");
        }
        static async Task<Uri> CreateProductAsync(Product product) {
            HttpResponseMessage response = await client.PostAsJsonAsync("api/product", product);
            //Throws an exception if the System.Net.Http.HttpResponseMessage.IsSuccessStatusCode
            //property for the HTTP response is false.
            response.EnsureSuccessStatusCode();
            return response.Headers.Location;
        }
        static async Task<Product> GetProductAsync(string path)
        {
            Product product = null;
            ///send a get request
            HttpResponseMessage response = await client.GetAsync(path);
            if (response.IsSuccessStatusCode) {
                product = await response.Content.ReadAsAsync<Product>();
            }
            return product;
        }

        static async Task<Product> UpdateProductAsync(Product product) {
            HttpResponseMessage response = await client.PutAsJsonAsync($"api/product/{product.Id}", product);
            response.EnsureSuccessStatusCode();
            // Deserialize the updated product from the response body.
            product = await response.Content.ReadAsAsync<Product>();
            return product;
        }
        static async Task<HttpStatusCode> DeleteProductAsync(string id)
        {
            HttpResponseMessage response = await client.DeleteAsync(
                $"api/products/{id}");
            return response.StatusCode;
        }

        static async Task RunAsync()
        {
            client.BaseAddress = new Uri("http://localhost:55919");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            try
            {
                // Create a new product
                Product product = new Product
                {
                    Nombre="gizmo",
                    PrecioCosto=90,
                    PrecioVenta= 100,
                    AplicaImpuesto=false,
                    Codigo="Gixmo",
                    Descripcion="Gixmo",
                    Estado=1,
                    RamoProductoId=1
                };

                var url = await CreateProductAsync(product);
                Console.WriteLine($"Created at {url}");

                // Get the product
                product = await GetProductAsync(url.PathAndQuery);
                ShowProduct(product);

                // Update the product
                Console.WriteLine("Updating price...");
                product.PrecioCosto = 80;
                await UpdateProductAsync(product);

                // Get the updated product
                product = await GetProductAsync(url.PathAndQuery);
                ShowProduct(product);

                // Delete the product
                //var statusCode = await DeleteProductAsync(product.Id);
                //Console.WriteLine($"Deleted (HTTP Status = {(int)statusCode})");

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            Console.ReadLine();
        }
    }
}

