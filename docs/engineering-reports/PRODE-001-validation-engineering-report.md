# Informe de ingeniería — PRODE-001: rechazo de goles negativos

- Fecha: 2026-10-26.
- Referencia: [Issue #1](https://github.com/DiegoDX/ProdeChampIA/issues/1), [plan actualizado](../stories/PRODE-001/implementation-plan.md), [informe previo](PRODE-001-engineering-report.md).
- Resultado: **Implementación y verificación local completas para el alcance aprobado.** Revisión independiente, aprobación humana de la entrega y publicación pendientes.
- Límite de evidencia: árbol de trabajo de `feature/PRODE-001-scoring-engine`, HEAD `9d6b4764891eec2053da78dd251b17a9439bd88f`, sin commits nuevos. Los cambios locales previos se conservaron.

## 1. Resultado y autorización

El usuario confirmó expresamente la propuesta de rechazar cualquier valor negativo en los cuatro parámetros `int` del calculador mediante `ArgumentOutOfRangeException`, identificando el parámetro inválido. Esa confirmación resuelve la ambigüedad registrada en el informe anterior y autoriza este seguimiento TDD. No se cambiaron la Issue ni sus cinco criterios originales; la regla adicional se documenta como aprobación en la conversación.

Se añadieron 10 casos de validación antes de editar producción. RED: los 34 casos originales pasaron y los 10 nuevos fallaron por ausencia de excepción. Después de incorporar cuatro guardas de .NET antes de puntuar, las 44 pruebas pasaron. El build de los seis proyectos y la regresión completa terminaron correctamente. La solución incluye este informe y el proyecto UnitTests marca los archivos Scoring como visibles sin duplicar su compilación.

## 2. Trazabilidad

| Requisito | Estado | Implementación | Evidencia |
| --- | --- | --- | --- |
| AC1/AC5: marcador exacto devuelve solo 5 | Verificado | Regla de puntuación original preservada tras la validación | Casos E01–E04/B01–B05 de QA; suite original aprobada |
| AC2: ganador correcto devuelve 3 | Verificado | Comparación de desenlaces preservada | W01 y teoría de ganador correcto aprobadas |
| AC3: empate correcto no exacto devuelve 3 | Verificado | Comparación de desenlaces preservada | D01 y teoría de empate correcto aprobadas |
| AC4: resultado incorrecto devuelve 0 | Verificado | Retorno original de 0 preservado | I01 y teoría de resultado incorrecto aprobadas |
| Rechazar cada argumento negativo e identificarlo | Verificado | `ThrowIfNegative` para los cuatro argumentos | Ocho casos de la teoría `Calculate_WithNegativeGoal_ThrowsArgumentOutOfRangeExceptionForParameter`: -1 e `int.MinValue` en cada posición |
| Validar antes de devolver premios | Verificado | Guardas antes de comprobar marcador exacto o desenlace | Dos casos de `Calculate_WithExactNegativeScore_RejectsBeforeAwardingPoints` |
| Cero y valores no negativos siguen siendo válidos | Verificado para escenarios cubiertos | Las guardas rechazan únicamente valores menores que cero | Casos originales de cero e `int.MaxValue` aprobados |

Las pruebas con varios argumentos negativos exigen rechazo, sin convertir el orden de validación en un contrato de negocio. Los nombres se comprueban cuando existe un único argumento inválido. No se exige un texto de excepción dependiente del idioma del runtime.

## 3. Archivos cambiados en este seguimiento

| Archivo | Cambio |
| --- | --- |
| `src/ProdeChamp.Domain/Scoring/PredictionPointsCalculator.cs` | Cuatro llamadas a `ArgumentOutOfRangeException.ThrowIfNegative` antes de la puntuación. |
| `tests/ProdeChamp.UnitTests/Scoring/PredictionPointsCalculatorValidationTests.cs` | Archivo nuevo con dos teorías y 10 casos de validación. |
| `tests/ProdeChamp.UnitTests/ProdeChamp.UnitTests.csproj` | `Compile Update` con `Visible=true` para `Scoring\*.cs`; conserva inclusión automática, referencias y paquetes. |
| `ProdeChamp.slnx` | Nuevo informe como solution item en `docs > engineering-reports`; proyectos y documentos existentes conservados. |
| `docs/stories/PRODE-001/implementation-plan.md` | Aprobación explícita, plan TDD y estado Developer/GREEN actual. |
| `docs/stories/PRODE-001/qa-report.md` | Nota actualizada, manteniendo el cuerpo histórico de QA/RED. |
| `docs/engineering-reports/PRODE-001-engineering-report.md` | Estado actual, trazabilidad de validación y enlace a este seguimiento; historial conservado. |
| `docs/engineering-reports/PRODE-001-validation-engineering-report.md` | Este informe y preguntas de entrevista. |
| `README.md`, `docs/README.md`, `docs/architecture/README.md`, `docs/product/README.md`, `docs/testing/README.md`, `docs/user-stories/README.md` | Contrato aprobado, 44 pruebas y revisión pendiente sincronizados. |

El archivo original `PredictionPointsCalculatorTests.cs` se conservó byte por byte: SHA-256 `63CFF09C75754786CA027B0CB63E61B5571C3FE4601832101F9E014118D8A2F7`. El proyecto Domain ya tenía metadata de visibilidad; no se modificó en esta etapa. No se tocaron AGENTS.md, Application, Infrastructure ni API en este seguimiento.

## 4. Decisiones técnicas

- Mantener la validación dentro de Domain garantiza el contrato aun cuando el llamador no valide los datos.
- Usar `ArgumentOutOfRangeException.ThrowIfNegative` de la biblioteca estándar .NET evita helpers o dependencias nuevas. El nombre del argumento queda identificado mediante el mecanismo estándar de la API, verificado por pruebas.
- Ejecutar todas las guardas antes de puntuar evita otorgar 5 a marcadores exactos inválidos. No hay clamping ni conversión de un dato inválido en un premio de 0.
- La firma síncrona, ausencia de estado, comparación con `CompareTo` y reglas 5/3/3/0 se conservaron. No se introdujeron capas, interfaces, endpoints ni restricciones superiores de goles.
- No fue necesaria refactorización adicional: cuatro guardas directas expresan la regla de forma explícita y breve.

## 5. Verificación ejecutada

Se recuperó correctamente la Issue mediante `gh issue view 1 --repo DiegoDX/ProdeChampIA --json number,title,url,body,state,labels,comments,updatedAt`: abierta, sin comentarios, criterios originales sin cambios.

Se utilizaron assets ya restaurados y estas variables de proceso:

```powershell
$env:DOTNET_CLI_HOME=Join-Path (Get-Location) '.local/dotnet'
$env:DOTNET_SKIP_FIRST_TIME_EXPERIENCE='1'
$env:DOTNET_CLI_TELEMETRY_OPTOUT='1'
$env:DOTNET_ADD_GLOBAL_TOOLS_TO_PATH='0'
```

### RED

```powershell
dotnet test tests/ProdeChamp.UnitTests/ProdeChamp.UnitTests.csproj --no-restore --filter FullyQualifiedName~PredictionPointsCalculator --logger 'trx;LogFileName=PRODE-001-validation-red.trx' --results-directory tests/ProdeChamp.UnitTests/TestResults --verbosity minimal
```

**Exit 1: 44 ejecutadas, 34 aprobadas, 10 fallidas, 0 omitidas.** Compilación y descubrimiento correctos. Los 10 casos nuevos fallaron con `Assert.Throws() Failure: No exception was thrown`, esperando `ArgumentOutOfRangeException`. Se corroboraron contadores y mensajes leyendo el TRX antes de editar producción. El fallo es de comportamiento y no de entorno.

### GREEN

```powershell
dotnet test tests/ProdeChamp.UnitTests/ProdeChamp.UnitTests.csproj --no-restore --filter FullyQualifiedName~PredictionPointsCalculator --logger 'trx;LogFileName=PRODE-001-validation-green.trx' --results-directory tests/ProdeChamp.UnitTests/TestResults --verbosity minimal
```

**Exit 0: 44 aprobadas, 0 fallidas, 0 omitidas.** Las pruebas originales y las nuevas pasaron tras añadir las guardas. No se modificaron pruebas para obtener Green.

### Build y regresión

```powershell
dotnet build ProdeChamp.slnx --no-restore --verbosity minimal
dotnet test ProdeChamp.slnx --no-build --no-restore --logger 'trx;LogFilePrefix=PRODE-001-regression' --results-directory tests/TestResults --verbosity minimal
```

**Build: exit 0, seis proyectos, 0 advertencias y 0 errores. Regresión: exit 0, 44 unitarias aprobadas, 0 fallidas y 0 omitidas. Integración: cero pruebas descubiertas.** Los comandos de tests/build se ejecutaron con aprobación fuera del sandbox, porque los artefactos existentes requieren esos permisos. No se hizo restore ni acceso a paquetes en línea.

Evidencia ignorada conservada: `tests/ProdeChamp.UnitTests/TestResults/PRODE-001-validation-red.trx`, `PRODE-001-validation-green.trx` y los TRX de regresión `tests/TestResults/PRODE-001-regression_net10.0_20261026172035.trx` (unitarias) y `...172036.trx` (integración vacía). Los artefactos anteriores se conservaron. La regresión final repetida se conserva en `tests/TestResults/PRODE-001-regression_net10.0_20261026180309.trx` (44 unitarias aprobadas) y `...180308.trx` (integración vacía).

### Reconciliación final del editor y del disco

La inspección final detectó que el archivo del calculador había vuelto a la versión sin las guardas después del primer Green. La causa del cambio no quedó determinada; no se atribuye al usuario ni se supone que un buffer lo produjo. Se contrastó el editor, se recargó la solución mediante el aviso de modificación externa y se cerró la pestaña de la versión anterior antes de restaurar las cuatro guardas aprobadas. Se repitieron build y regresión con los mismos comandos: build exit 0, 0 advertencias/errores; 44 unitarias aprobadas, 0 fallidas/omitidas y cero integración. El SHA-256 del calculador fue idéntico antes y después de esta ejecución final: `AF99C9E17951AD35BA9D29E7792BDB81C20A843D90119F1E846F5EF398F024B7`. El resultado final corresponde a la fuente en disco verificada, no solamente al primer binario Green.

Verificación estructural y revisión local: `dotnet msbuild tests/ProdeChamp.UnitTests/ProdeChamp.UnitTests.csproj -getItem:Compile -nologo` terminó con exit 0 y listó exactamente los dos archivos Scoring con `Visible=true`; el equivalente de Domain listó una sola entrada del calculador con `Visible=true`. `dotnet sln ProdeChamp.slnx list` conservó los seis proyectos. La comprobación XML confirmó solution items únicos y rutas existentes; se comprobó el nuevo informe en el árbol de Visual Studio después de recargar. La visibilidad de las pruebas se verificó mediante metadata de MSBuild; no se afirma inspección visual de los dos archivos expandidos. `git diff --check` terminó con exit 0, y una revisión de 13 archivos no encontró marcadores de conflicto, espacios finales ni enlaces locales rotos. Se inspeccionaron las guardas y las pruebas, se mantuvo el hash de la suite original y no se cambiaron referencias de proyectos ni paquetes.

## 6. Límites y revisión pendiente

- Independiente code review y aprobación humana de la entrega pendientes; este informe no concede aprobación externa.
- No se ejecutó CI para el árbol de trabajo actual ni se midió porcentaje de cobertura.
- El proyecto de integración no contiene pruebas; no hay verificación de API en ejecución, SQL Server ni frontend, fuera del alcance.
- No se modificó la Issue remota. La regla adicional tiene aprobación explícita en la conversación y queda registrada localmente.
- No se crearon commits, pushes, Pull Requests ni merges. La publicación es una etapa posterior y requiere autorización.

## 7. Preguntas de entrevista

1. **¿Por qué validar antes del marcador exacto?** Una entrada inválida no debe obtener un premio aunque ambas parejas coincidan. Los dos casos de marcador exacto negativo capturan esa regresión.
2. **¿Por qué comprobar `ParamName` y no el mensaje completo?** El parámetro es parte del contrato aprobado; el texto puede variar según el runtime o su idioma. Ver la teoría de los ocho argumentos negativos.
3. **¿Qué aporta `int.MinValue` además de -1?** Comprueba el extremo inferior del contrato `int`, evitando validaciones incompletas o basadas en negación aritmética. La implementación usa comparación estándar sin aritmética adicional.
4. **¿Qué demuestra que no se debilitó la suite original para lograr Green?** Su hash sigue igual, y el mismo run Red muestra 34 casos originales aprobados mientras fallan los 10 nuevos; Green aprueba los 44.

## 8. Referencias

- [Calculador](../../src/ProdeChamp.Domain/Scoring/PredictionPointsCalculator.cs), [pruebas originales](../../tests/ProdeChamp.UnitTests/Scoring/PredictionPointsCalculatorTests.cs), [pruebas de validación](../../tests/ProdeChamp.UnitTests/Scoring/PredictionPointsCalculatorValidationTests.cs).
- [AGENTS.md](../../AGENTS.md), [backend-implementation](../../.agents/skills/backend-implementation/SKILL.md), [tdd-implementation](../../.agents/skills/tdd-implementation/SKILL.md), [engineering-report](../../.agents/skills/engineering-report/SKILL.md).
