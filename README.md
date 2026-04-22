# Mine Seeker

Simulación de mineros con **pathfinding** y **finite state machines** desarrollada en Unity 6 (6000.0.36f1) como Trabajo Práctico 1 de la materia *Inteligencia Artificial con Unity* — Tecnicatura Superior en Programación de Videojuegos con Motores.

La consigna completa se encuentra en [`Docs/TP1_Consigna.md`](Docs/TP1_Consigna.md).

## Estado del TP

- ✅ **Tier B (4)** — pathfinding básico, FSM personalizada, ciclo completo de minado, obstáculos.
- ✅ **Tier A (7)** — selección de algoritmo en runtime (DFS, BFS, Dijkstra, A*), estados `Idle`/`GoToMine`/`Mining`/`Unload`, UI de oro total y por minero.
- ✅ **Tier S (10)** — recolección/descarga progresiva con corutinas, zonas de costo variable (agua/arena), enemigos con FSM propia (`Wander`/`Chase`/`Attack`) e interacción con mineros (`Escape`/`Death` + respawn).

## Controles

- **Cámara**: mover el mouse a los bordes de la pantalla panea el mapa; rueda del mouse hace zoom in/out.
- **`T`**: togglea la UI de estadísticas on/off.
- **Dropdown de Strategy**: cambia el algoritmo de pathfinding en tiempo de ejecución.

## Estructura de carpetas

```
mine_seeker/
├── Assets/
│   ├── _Project/                  ← Contenido propio del proyecto
│   │   ├── Art/                   (materiales, modelos, texturas propios)
│   │   ├── Data/                  (ScriptableObjects: MinerStats, EnemyStats)
│   │   ├── Prefabs/
│   │   │   ├── Agents/            (Miner, Enemy)
│   │   │   ├── Map/               (Mine, Base, zonas de costo)
│   │   │   └── UI/                (MinerRow)
│   │   ├── Scenes/                (MineSeeker)
│   │   └── Scripts/
│   │       ├── Agents/
│   │       │   ├── Miner/         (Miner, MinerStats, MinerContext + estados)
│   │       │   └── Enemy/         (Enemy, EnemyStats + estados)
│   │       ├── Camera/            (CameraController — pan por bordes + zoom)
│   │       ├── Common/            (MonoBehaviourSingleton)
│   │       ├── Core/              (GameManager)
│   │       ├── FSM/               (FiniteStateMachine, FsmState, DoubleEntryTable)
│   │       ├── Map/               (Base, Mine, TerrainZone)
│   │       ├── PathFinding/       (PathNode, agente, manager, estrategias BFS/DFS/Dijkstra/A*)
│   │       └── UI/                (UIManager, MinerRow)
│   │
│   └── ThirdParty/                ← Assets de terceros importados
│
├── Docs/                          ← Documentación
│   └── TP1_Consigna.md
│
├── Packages/
└── ProjectSettings/
```

## Arquitectura

### Pathfinding

- Grid de `PathNode`s generada por raycast desde arriba del mapa (`PathNodeGenerator`), respetando tags de terreno para asignar `CostMultiplier` por nodo.
- Estrategias implementadas manualmente en `Scripts/PathFinding/Strategies/`: `BreadthFirstStrategy`, `DepthFirstStrategy`, `DijkstraStrategy`, `AStarStrategy`.
- `PathFindingManager` es un singleton que expone `CreatePath(origin, destination)` y `SetStrategy(EPathFindingStrategy)`.
- `PathNodeAgent` se encarga de seguir el path nodo a nodo aplicando `movementSpeed` con ajuste por terreno activo (`TerrainZone`).

### FSM

- `FiniteStateMachine<T>` genérica parametrizada por el Owner (Miner o Enemy), con transiciones configuradas vía `DoubleEntryTable` (estado + evento → estado).
- Estados concretos serializables para que los parámetros se configuren desde el Inspector.

### Miner (8 estados)

`Idle → GoToMine → Mining → ReturnToBase → Unload → Idle`, con dos estados reactivos: `Escape` (al recibir daño) y `Death` (al morir; respawnea en la base tras `RespawnTime`).

### Enemy (3 estados)

`Wander ↔ Chase ↔ Attack` con detección del minero más cercano vía `GameManager.Miners`. Patrulla en círculo alrededor de un `HomePoint` fijo para evitar que los enemigos se acumulen cerca de la base.

### Zonas de costo

`TerrainZone` (BoxCollider trigger) notifica al `PathNodeAgent` cuando un agente entra/sale, el cual multiplica su velocidad por el valor del menor (más lento) de los multiplicadores activos. Simultáneamente, el `PathNodeGenerator` lee el `tag` del collider para asignar `CostMultiplier` en los nodos, de modo que Dijkstra y A* los evitan cuando hay alternativa más barata.

## Requisitos

- Unity **6000.0.36f1** o superior.
- Input System Package (new).

## Convenciones

- **`_Project/`**: el guion bajo lo mantiene al tope de la lista en el Project window. Todo lo creado por el autor vive acá.
- **`ThirdParty/`**: aislar los packs importados evita que se mezclen con el código propio y facilita actualizarlos o removerlos.
- **`Scripts/`** dividido por dominio (FSM, PathFinding, Agents, Map, UI, Camera, Core, Common) para reflejar la arquitectura.
