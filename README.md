# Switch Protocol — Shooter Arena (Unity)

**Autor:** Matias Pulido  
**Motor:** Unity 2022.3.63f3  
**Plataforma:** Windows

---

## 🎮 Descripción

**Switch Protocol** es un shooter 3D en una **arena** donde tomas el rol de un soldado pelado y estas completamente SOLO en un campo de arena militar invadido por monstruos, y tu tarea es limpiarlo lo antes posible antes de que se termine el tiempo y 
se vea infestado.

Tenes un rifle y un lanza cohetes, aprovecha tus herramientas a tu disposicion para terminar con la invasión!

---

## 🧩 Gameplay Loop

1. Elegís **dificultad** desde el menú.
2. Entrás a la arena y enfrentás enemigos.
3. Alternás entre **armas** según la situación (Rifle o Lanzacohetes)
4. Ganas si llegas al puntaje dependiendo la dificultad, o perdes si tu vida llega a 0 o el tiempo se termina.

---



## 🆕 NUEVAS IMPLEMENTACIONES (desde Parcial 2)

Esta sección agrupa todo lo agregado/mejorado respecto a la última versión evaluada (Parcial 2).

### Presentación y UX
- Boot screen con identificación del estudiante después de “Made with Unity”
- Pantallas de **Victoria** y **Derrota** con transición
- Ajustes de presentación general para publicación

### Cámaras y Control
- Sistema de cámaras con **Cinemachine**
- Mejora de camara primera persona.
- Ajustes de control para jugabilidad equivalente en mouse/keyboard y gamepad

### Configuración
- Menú de **Resolución** y **Framerate máximo**

### Feedback y Polishing
- Feedback positivo / negativo / neutral para interacciones relevantes
- Postprocesado estático
- Postprocesado por zonas
- Skybox personalizado acorde al tono del juego
- Shaders personalizados en las pantallas de victoria/derrota

### Progresión / Sistemas
- Sistema de **guardado entre partidas**
- Sistema de **logros**:
  - Logros desbloqueables por acciones
  - Visualización de logros en UI
  - Notificaciones al desbloquear

### Arquitectura
- Sistema centralizado de eventos para comunicación entre gameplay/UI/sistemas

---

## 🎛️ Controles

### Teclado y Mouse
- **WASD:** mover
- **Mouse:** mirar/apuntar
- **Click Izquierdo:** disparar
- **NUEVO** **Click Derecho:** apuntar (ADS)
- **Q / Wheel:** alternar arma *(ajustar según implementación)*
- **ESC:** pausa
- **NUEVO** **Shift:** correr
- **ESC:** pausa


### Gamepad (Xbox / genérico)
- **Left Stick:** mover
- **Right Stick:** mirar/apuntar
- **RT (R2):** disparar
- **LT (L2):** apuntar
- **RB/LB o D-Pad:** cambiar arma
- **Start:** pausa
- **A:** confirmar / interactuar (UI)

---

## 🏆 Condiciones de Victoria / Derrota

- **Victoria:** Llegar a cierta cantidad de puntos dependiendo la dificultad.
- **Derrota:** Tu vida llego a 0 o el tiempo del juego se termino.

---

## 💾 Guardado

El juego guarda progreso relevante entre partidas (configuración / logros / progreso)

---

## 📦 Instalación (Windows)

1. Ir a **Releases**
2. Descargar el `.zip`
3. Extraer en una carpeta
4. Ejecutar `SwitchProtocol.exe`

---

## 🔗 Links

- **Itch.io:** *[(link)](https://1pkure.itch.io/switch-protocol)*

---

## 🧾 Créditos / Assets

- Todos los creditos estan dentro del juego en su respectiva escena!

---

## 🧠 Posibles ampliaciones

- Nuevos tipos de enemigos (patrones combinados, IA más agresiva)
- Más armas o mods (silenciador, burst, carga)
- Desafíos diarios / modo endless
- Más arenas con variantes de cobertura y verticalidad
- Sistema de skins (ya vienen varios tipos de personajes incluidos en el paquete)
- Pickupeables como Healthpackage o Boost
