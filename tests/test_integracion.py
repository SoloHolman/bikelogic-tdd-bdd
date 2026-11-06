import pytest

from bikelogic.ordenes_compra.repositorio import RepositorioOrdenesEnMemoria
from bikelogic.ordenes_compra.servicio import ServicioOrdenesCompra
from bikelogic.proveedores.excepciones import ProveedorConOrdenesActivas
from bikelogic.proveedores.repositorio import RepositorioProveedoresEnMemoria
from bikelogic.proveedores.servicio import ServicioProveedores

NIT = "900123456"


@pytest.fixture
def ordenes():
    servicio = ServicioOrdenesCompra(RepositorioOrdenesEnMemoria())
    servicio.registrar_proveedor(NIT, 10_000_000)
    return servicio


@pytest.fixture
def proveedores(ordenes):
    servicio = ServicioProveedores(
        RepositorioProveedoresEnMemoria(),
        tiene_ordenes_activas=ordenes.tiene_ordenes_activas,
    )
    servicio.registrar(NIT, "ACME S.A.S.", "contacto@acme.com")
    return servicio


def test_proveedor_con_orden_pendiente_no_se_puede_desactivar(proveedores, ordenes):
    ordenes.registrar_orden("OC-100", NIT, 5_000_000)

    with pytest.raises(ProveedorConOrdenesActivas):
        proveedores.desactivar(NIT)


def test_proveedor_con_ordenes_aprobadas_si_se_puede_desactivar(proveedores, ordenes):
    ordenes.registrar_orden("OC-100", NIT, 5_000_000)
    ordenes.aprobar("OC-100")

    assert proveedores.desactivar(NIT).estado == "INACTIVO"
