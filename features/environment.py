"""Configuración de behave: ruta del código y utilidades compartidas por los pasos."""
import sys
from pathlib import Path

sys.path.insert(0, str(Path(__file__).resolve().parents[1] / "src"))

from bikelogic.errores import ErrorDeNegocio  # noqa: E402


def _ejecutar(context, accion, mensaje_exito):
    """Ejecuta una acción de negocio y deja el mensaje resultante en el contexto."""
    context.error_negocio = None
    try:
        accion()
        context.mensaje = mensaje_exito
    except ErrorDeNegocio as error:
        context.error_negocio = error
        context.mensaje = str(error)


def before_all(context):
    context.ejecutar = _ejecutar
