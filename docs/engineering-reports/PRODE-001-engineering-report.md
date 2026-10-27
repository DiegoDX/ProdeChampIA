# Informe de ingeniería — PRODE-001

- Fecha: 2026-10-26.
- Referencia: [Issue #1](https://github.com/DiegoDX/ProdeChampIA/issues/1), [plan](../stories/PRODE-001/implementation-plan.md), [QA/RED histórico](../stories/PRODE-001/qa-report.md).
- Resultado actual: **Implementación y validación verificadas localmente; revisión independiente y aprobación humana pendientes.** Los cinco criterios originales y el rechazo de negativos están implementados. Véase el [informe de validación](PRODE-001-validation-engineering-report.md): RED con 34 aprobadas/10 fallidas, GREEN y regresión con 44 aprobadas, build sin errores ni advertencias.

> Este documento conserva el historial de la primera etapa de puntuación y los seguimientos de visibilidad. Los resultados de 34 pruebas y referencias a validación pendiente en las secciones históricas describen momentos anteriores a la aprobación e implementación final del rechazo de negativos. El informe de validación enlazado contiene la evidencia actual.
- Evidencia: árbol de trabajo de `feature/PRODE-001-scoring-engine`, HEAD `9d6b4764891eec2053da78dd251b17a9439bd88f`. Cambios sin commit.

## 1. Resultado y autorización

El usuario aprobó explícitamente el cierre de QA/RED y el calculador síncrono en Domain con cuatro parámetros `int`, sin interfaz, endpoint ni dependencias nuevas. Se implementó la puntuación de 5/3/3/0 sin acumulación. Se preservaron las 34 pruebas existentes y los cambios locales previos.

El usuario indicó también que los goles no pueden ser negativos y posteriormente confirmó el rechazo mediante `ArgumentOutOfRangeException` con nombre del parámetro. Esa validación ya está implementada y verificada mediante 10 pruebas adicionales. La Issue y sus cinco criterios originales permanecen sin cambios; la aprobación adicional está registrada en la conversación, el plan y el informe de validación.

## 2. Trazabilidad de criterios

| Criterio original | Estado | Implementación | Pruebas existentes |
| --- | --- | --- | --- |
| AC1: 2-1 / 2-1 devuelve 5 | Verificado | Comparación exacta de ambos goles y retorno temprano | E01; teoría de marcador exacto |
| AC2: 2-1 / 3-0 devuelve 3 | Verificado | Igualdad de los resultados de `CompareTo` | W01; teoría de ganador correcto |
| AC3: 1-1 / 2-2 devuelve 3 | Verificado | Ambos resultados de comparación son cero | D01; teoría de empate correcto |
| AC4: 2-0 / 0-2 devuelve 0 | Verificado | Retorno 0 cuando difieren los desenlaces | I01; teoría de resultado incorrecto |
| AC5: categorías no acumulables | Verificado | Retorno 5 antes de comparar desenlaces; único premio por llamada | E01–E04, B01–B05 y reutilización R01 |
| Instrucción adicional: no permitir goles negativos | Verificado | Cuatro guardas `ArgumentOutOfRangeException.ThrowIfNegative` antes de puntuar | 10 pruebas nuevas aprobadas; ver informe de validación |

Los ID corresponden al informe de QA. Las cinco teorías contienen 33 casos y el único `Fact` cubre reutilización: 34 pruebas en total.

## 3. Archivos cambiados

| Archivo | Cambio en esta etapa |
| --- | --- |
| `src/ProdeChamp.Domain/Scoring/PredictionPointsCalculator.cs` | Sustitución del esqueleto por las reglas de puntuación. |
| `docs/stories/PRODE-001/implementation-plan.md` | Estado actual, aprobación, plan de ejecución y resultados; planificación original conservada como histórica. |
| `docs/stories/PRODE-001/qa-report.md` | Nota de transición a Developer/GREEN conservando la evidencia histórica Red. |
| `docs/engineering-reports/PRODE-001-engineering-report.md` | Este informe con evidencia, limitaciones y preguntas. |
| `docs/architecture/README.md` | Estado del calculador y límites de capas. |
| `docs/testing/README.md` | Suite real y distinción entre pruebas unitarias e integración vacía. |
| `docs/product/README.md`, `docs/user-stories/README.md`, `docs/README.md` | Reglas, enlaces y estado actual de la historia. |
| `README.md` | Calculador, 34 pruebas, diez Skills y limitación actual. |

`ProdeChamp.slnx` ya estaba modificado al comenzar; se preservó sin cambios adicionales (SHA-256 `651B0D38E8B82CDADF51390320B89F9754021D3960B0EFF6CE80DA81928102B0`). El archivo de pruebas existente tampoco se modificó (SHA-256 `63CFF09C75754786CA027B0CB63E61B5571C3FE4601832101F9E014118D8A2F7`). El plan, QA, calculador y pruebas ya estaban sin seguimiento al iniciar esta etapa. No se modificaron proyectos, dependencias ni configuración del SDK.

## 4. Decisiones técnicas

- Regla pura y síncrona en Domain: no realiza I/O ni requiere `async`, interfaz, registro de DI o infraestructura.
- El marcador exacto devuelve 5 inmediatamente. La comparación posterior determina victoria local, empate o victoria visitante y devuelve 3 o 0; así se evita acumular premios.
- `CompareTo` evita sumas o restas innecesarias y funciona en los límites enteros probados. No se inventó un máximo de goles.
- El calculador no retiene estado entre llamadas. No se añadieron abstracciones ni refactorizaciones adicionales porque la regla cabe en un método breve.
- La aclaración sobre negativos limita únicamente esa parte; el trabajo independiente sobre los criterios originales continuó según las Skills aplicadas.

## 5. Verificación ejecutada

Se recuperó correctamente la Issue mediante `gh issue view 1 --repo DiegoDX/ProdeChampIA --json number,title,url,body,state,labels,comments,updatedAt`. Sigue abierta, sin comentarios y sin cambios respecto de los criterios del plan.

Variables de proceso utilizadas para .NET:

```powershell
$env:DOTNET_CLI_HOME=Join-Path (Get-Location) '.local/dotnet'
$env:DOTNET_SKIP_FIRST_TIME_EXPERIENCE='1'
$env:DOTNET_CLI_TELEMETRY_OPTOUT='1'
$env:DOTNET_ADD_GLOBAL_TOOLS_TO_PATH='0'
```

**Red, antes de editar producción:**

```powershell
dotnet test tests/ProdeChamp.UnitTests/ProdeChamp.UnitTests.csproj --no-restore --filter FullyQualifiedName~PredictionPointsCalculatorTests --logger 'trx;LogFileName=PRODE-001-developer-red.trx' --results-directory tests/ProdeChamp.UnitTests/TestResults --verbosity minimal
```

El intento restringido falló por `MSB3491` al escribir un artefacto de cobertura; no representa Red. El mismo comando con ejecución aprobada fuera del sandbox terminó con **exit 1; 34 ejecutadas, 34 fallidas, 0 aprobadas, 0 omitidas**. Se inspeccionó el TRX: los 34 mensajes son `System.NotImplementedException`. Compilación y descubrimiento habían terminado correctamente.

**Green, después de implementar puntuación:**

```powershell
dotnet test tests/ProdeChamp.UnitTests/ProdeChamp.UnitTests.csproj --no-restore --filter FullyQualifiedName~PredictionPointsCalculatorTests --logger 'trx;LogFileName=PRODE-001-developer-green.trx' --results-directory tests/ProdeChamp.UnitTests/TestResults --verbosity minimal
```

El intento restringido falló por `CS2012` al escribir la DLL de Domain; no representa un fallo de comportamiento. La ejecución aprobada fuera del sandbox terminó con **exit 0; 34 aprobadas, 0 fallidas, 0 omitidas**. Contadores corroborados leyendo el TRX. No se alteraron pruebas para lograr Green.

**Compilación y regresión de solución:**

```powershell
dotnet build ProdeChamp.slnx --no-restore --verbosity minimal
dotnet test ProdeChamp.slnx --no-build --no-restore --logger 'trx;LogFilePrefix=PRODE-001-regression' --results-directory tests/TestResults --verbosity minimal
```

Ambos comandos se ejecutaron fuera del sandbox con aprobación. Build: **exit 0, seis proyectos, 0 advertencias y 0 errores**. Test: **exit 0; 34 pruebas unitarias aprobadas, 0 fallidas, 0 omitidas; cero pruebas de integración descubiertas**. No se realizó restore ni acceso a paquetes en línea; se usaron los assets existentes.

Los TRX nuevos se conservan en las carpetas ignoradas `tests/ProdeChamp.UnitTests/TestResults/` y `tests/TestResults/`; se preservaron los TRX históricos de QA.

Revisión final: `git -c safe.directory=C:/ProdeChampIA/ProdeChampIA diff --check` terminó con exit 0. Se inspeccionó el código y el diff de documentación, y una comprobación PowerShell de los 11 archivos revisados confirmó ausencia de marcadores de conflicto, espacios finales y enlaces Markdown locales rotos. La primera versión de esa comprobación no resolvía los enlaces del README raíz; se corrigió el script y se ejecutó de nuevo correctamente. Los hashes anteriores confirman que las pruebas y la solución preexistentes se conservaron. Las referencias de proyectos y paquetes siguen siendo las inspeccionadas al inicio.

## 6. Limitaciones y próximos pasos

- El contrato de negativos quedó resuelto con aprobación del usuario; su ciclo Red/Green y la regresión de 44 pruebas están documentados en el informe de validación. No queda ambigüedad de validación pendiente.
- Revisión independiente de código y aprobación humana pendientes. La inspección de implementación no equivale a aprobación externa.
- No se midió porcentaje de cobertura. No hay pruebas de integración, endpoint ni comprobación de API en ejecución, SQL Server o frontend.
- CI de esta versión no ejecutado. Las verificaciones informadas son locales; las ejecuciones históricas de `main` no prueban estos cambios.
- No se hicieron commits, pushes, modificaciones de Issues, Pull Requests ni merges. La publicación requiere una etapa posterior autorizada.

## 7. Preguntas de entrevista

1. **¿Cómo evita el método acumular 8 puntos?** Retorna 5 al verificar ambos goles exactos antes de evaluar el desenlace. Referencias: calculador y teoría `Calculate_WithExactScore_ReturnsFiveWithoutAddingOutcomePoints`.
2. **¿Qué ventaja tiene `CompareTo` en esta regla?** Expresa las tres posibilidades del resultado sin aritmética innecesaria; la teoría `Calculate_AtIntegerBoundaries_ReturnsExpectedPoints` cubre extremos de `int`.
3. **¿Qué demuestra Red y qué no?** El TRX demuestra ejecución contra comportamiento ausente; errores de permisos o compilación no establecen Red. Comparar los archivos `PRODE-001-developer-red.trx` y `PRODE-001-developer-green.trx`.
4. **¿Por qué un Green con 34 pruebas no completa la nueva prohibición de negativos?** Las pruebas existentes verifican puntuación para valores no negativos; un contrato adicional requiere decisión, pruebas y validación propias. Referencias: plan y limitaciones de este informe.

## 8. Referencias

- [AGENTS.md](../../AGENTS.md), [backend-implementation](../../.agents/skills/backend-implementation/SKILL.md), [tdd-implementation](../../.agents/skills/tdd-implementation/SKILL.md), [engineering-report](../../.agents/skills/engineering-report/SKILL.md).
- [Calculador](../../src/ProdeChamp.Domain/Scoring/PredictionPointsCalculator.cs) y [pruebas existentes](../../tests/ProdeChamp.UnitTests/Scoring/PredictionPointsCalculatorTests.cs).
- [Arquitectura](../architecture/README.md), [estrategia de pruebas](../testing/README.md) y [preguntas de Sprint 0](../interview-questions/sprint-0.md), conservadas como evidencia histórica.

## 9. Seguimiento — visibilidad en Visual Studio

El 2026-10-26 el usuario pidió que los archivos creados siempre estén visibles en la solución. Se agregó este informe a `docs > engineering-reports` y `AGENTS.md` a `Solution Items` en `ProdeChamp.slnx`, conservando los documentos y proyectos existentes. Se registró la preferencia permanente en `AGENTS.md`: usar la inclusión del proyecto para código y solution items para documentos relevantes, sin duplicar entradas de compilación ni incluir artefactos generados.

Archivos cambiados en este seguimiento: `AGENTS.md`, `ProdeChamp.slnx` y este informe. La afirmación anterior de solución sin cambios y su hash describen la etapa Developer previa a este seguimiento. No se modificó el calculador ni las pruebas; la política pendiente de negativos continúa sin resolverse. No corresponde un nuevo ciclo TDD para esta organización de archivos; pruebas de comportamiento no ejecutadas en este seguimiento. `dotnet sln ProdeChamp.slnx list` terminó con exit 0 y conservó los seis proyectos. `dotnet msbuild src/ProdeChamp.Domain/ProdeChamp.Domain.csproj -getItem:Compile -nologo` y el equivalente de UnitTests terminaron con exit 0 y listaron los dos archivos C# de Scoring. La comprobación XML confirmó que todos los archivos y proyectos vinculados existen; `git diff --check` terminó con exit 0. No se inspeccionó la interfaz de Visual Studio; puede ser necesario recargar la solución abierta. Pregunta de entrevista adicional: **¿Por qué un archivo C# aparece por el proyecto y un informe requiere un solution item?** Los proyectos SDK incluyen código por sus patrones predeterminados; los solution items organizan documentos sin compilarlos.


## 10. Corrección de visibilidad del calculador

El usuario informó que no veía Scoring ni PredictionPointsCalculator.cs. Se comprobó en la interfaz de Visual Studio que Domain mostraba únicamente Dependencies, aunque el archivo existía y MSBuild lo incluía mediante los patrones del SDK. Se activó Mostrar todos los archivos para inspeccionarlo y se actualizó la fecha de Domain y UnitTests sin cambiar su contenido. Después se añadió a src/ProdeChamp.Domain/ProdeChamp.Domain.csproj la metadata explícita `<Compile Update="Scoring\PredictionPointsCalculator.cs" Visible="true" />`, sin agregar una inclusión Compile duplicada.

Verificación: Visual Studio mostró la carpeta Scoring y el archivo dentro de ProdeChamp.Domain, permitió abrir la clase y mantuvo el archivo visible tras desactivar Mostrar todos los archivos. `dotnet msbuild src/ProdeChamp.Domain/ProdeChamp.Domain.csproj -getItem:Compile -nologo` terminó con exit 0 y devolvió exactamente una entrada del calculador con Visible=true. `dotnet sln ProdeChamp.slnx list` terminó con exit 0 y conservó los seis proyectos. No se cambió la lógica en disco ni el archivo de pruebas (hash previo conservado). No se ejecutaron pruebas de comportamiento ni una nueva compilación: el cambio afecta la visibilidad del elemento existente.

Archivos cambiados en esta corrección: el proyecto Domain y este informe. El proyecto se guardó en UTF-8 sin BOM; no cambia su interpretación XML. El rechazo de negativos continúa pendiente y no se implementó en este seguimiento. Pregunta de entrevista: **¿Qué diferencia hay entre Compile Update y Compile Include?** Update modifica metadata de un elemento existente; Include añade elementos y puede duplicar los que el SDK ya descubre.
