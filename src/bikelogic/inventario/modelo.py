"""Modelo del módulo de inventario."""
from dataclasses import dataclass


@dataclass
class Producto:
    codigo: str
    categoria: str
    stock: int
