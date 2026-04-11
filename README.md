# Mine Seeker

Simulación de mineros con **pathfinding** y **finite state machines** desarrollada en Unity 6 (6000.0.36f1) como Trabajo Práctico 1 de la materia *Inteligencia Artificial con Unity* — Tecnicatura Superior en Programación de Videojuegos con Motores.

La consigna completa se encuentra en [`Docs/TP1_Consigna.md`](Docs/TP1_Consigna.md).

## Estructura de carpetas

```
mine_seeker/
├── Assets/
│   ├── _Project/              ← Todo el contenido propio del proyecto
│   │   ├── Art/
│   │   │   ├── Materials/
│   │   │   ├── Models/
│   │   │   └── Textures/
│   │   ├── Prefabs/
│   │   │   ├── Agents/        (Minero, Enemigo)
│   │   │   ├── Environment/   (Mina, Base, Obstáculos)
│   │   │   └── UI/
│   │   ├── Scenes/
│   │   ├── Scripts/
│   │   │   ├── Agents/        (Minero, Enemigo)
│   │   │   ├── FSM/           (StateMachine, State base, estados concretos)
│   │   │   ├── Pathfinding/   (Grid, Node, algoritmos: BFS/DFS/Dijkstra/A*)
│   │   │   ├── Map/           (Terreno, vetas, zonas de costo)
│   │   │   ├── UI/
│   │   │   └── Utils/
│   │   ├── Settings/          (ScriptableObjects de configuración)
│   │   └── UI/                (sprites, fonts propios)
│   │
│   ├── ThirdParty/            ← Assets de terceros importados
│   │   ├── Pandazole_Ultimate_Pack/
│   │   ├── Stylized NPC - Peasant Nolant/
│   │   ├── Veresen/
│   │   └── asoliddev - Low Poly Fantasy Warrior/
│   │
│   └── Settings/              (URP, Input System, etc.)
│
├── Docs/                      ← Documentación del proyecto
│   └── TP1_Consigna.md
│
├── Packages/
└── ProjectSettings/
```

## Convenciones

- **`_Project/`**: el guion bajo lo mantiene al tope de la lista en el Project window. Todo lo escrito/creado por el autor vive acá.
- **`ThirdParty/`**: aislar los packs importados evita que se mezclen con el código propio y facilita actualizarlos o removerlos.
- **`Scripts/`** está dividido por dominio (FSM, Pathfinding, Agents, Map, UI) para reflejar la arquitectura de la simulación.

## Requisitos

- Unity **6000.0.36f1** o superior.

## Tiers de la consigna

- **Tier B (4):** pathfinding básico, FSM personalizada, ciclo completo de minado, obstáculos.
- **Tier A (7):** selección de algoritmo en runtime (DFS, BFS, Dijkstra, A*), estados mínimos requeridos, UI de oro.
- **Tier S (10):** recolección/descarga progresiva con corutinas, zonas de costo variable, enemigos con FSM propia e interacción con mineros.
