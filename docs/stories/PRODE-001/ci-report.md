# CI — Skill y aplicación a PRODE-001

> Estado actualizado el 2026-10-27: revisión independiente **APPROVED**, publicación autorizada y [PR #3](https://github.com/DiegoDX/ProdeChampIA/pull/3) abierto hacia `main`. El commit de implementación `fe9155f` pasó GitHub Actions con 44 unitarias y TRX publicados. La revisión humana final sigue pendiente. Las secciones originales del 2026-10-26 conservan su contexto histórico; el seguimiento al final registra la evidencia remota.

- Fecha: 2026-10-26.
- Rama: `feature/PRODE-001-scoring-engine`; HEAD local: `9d6b476`.
- Alcance: crear una Skill de integración continua para ProdeChamp y aplicarla al workflow existente. Sin despliegue ni publicación.
- Resultado: **Skill creada y comprobada estructuralmente; CI configurado y verificado localmente en Release. Ejecución remota de esta versión pendiente.**

## Decisión: CI ahora, CD por separado

Se creó [ci/SKILL.md](../../../.agents/skills/ci/SKILL.md), siguiendo la Skill `skill-creator` y las convenciones del repositorio. La integración continua ya tiene un objetivo concreto: restaurar, compilar y probar la solución .NET 10. No hay un destino ni proceso de despliegue aprobados; CD requiere un alcance posterior con entorno, publicación y recuperación definidos. No se creó otra Skill de revisión: `code-review` ya existe y conserva su responsabilidad separada.

La nueva Skill distingue configuración local, verificación local y resultados de GitHub asociados a un commit. Requiere inspeccionar lo que ya existe, conservar la evidencia y los cambios previos, detectar ejecuciones vacías cuando se esperan pruebas y respetar los permisos de publicación de AGENTS.md. No concede aprobación de código ni de entrega, no implementa reglas de negocio y no incluye CD.

## Archivos cambiados en esta tarea

| Archivo | Cambio |
| --- | --- |
| `.agents/skills/ci/SKILL.md` | Nueva Skill autocontenida; no requiere scripts ni dependencias adicionales. |
| `.github/workflows/ci.yml` | Se amplió el CI existente para ramas de trabajo, evidencia TRX y comprobación de ejecución unitaria. |
| `ProdeChamp.slnx` | Skill, workflow e informe vinculados como solution items, conservando proyectos/documentos anteriores. |
| `README.md` | Inventario actualizado a 11 Skills y enlace a CI. |
| `docs/testing/README.md` | Instrucciones de uso y distinción entre CI local y remoto. |
| `docs/README.md` | Enlace a este informe. |
| `docs/stories/PRODE-001/ci-report.md` | Este informe, resultados, limitaciones y preguntas de entrevista. |

El archivo de producción y los dos archivos de pruebas no se modificaron en esta tarea. Se preservaron sus hashes respecto de la implementación verificada:

| Archivo | SHA-256 |
| --- | --- |
| `PredictionPointsCalculator.cs` | `AF99C9E17951AD35BA9D29E7792BDB81C20A843D90119F1E846F5EF398F024B7` |
| `PredictionPointsCalculatorTests.cs` | `63CFF09C75754786CA027B0CB63E61B5571C3FE4601832101F9E014118D8A2F7` |
| `PredictionPointsCalculatorValidationTests.cs` | `8B308C1DFA8942F12B1418C1AE3FB21D7C624040E4E3CFA18471B778D7441D70` |

## Primera aplicación de la Skill

Se leyó la Skill nueva antes de ejecutar su flujo. Se inspeccionaron AGENTS.md, la historia #1, plan, arquitectura, pruebas, SDK, solución, cambios locales y CI existente. El workflow ya ejecutaba restore/build/test en Release para `main` y PRs hacia `main`; no se reemplazó por otro duplicado.

Se mantuvieron Windows 2025, .NET 10 y las acciones existentes de checkout/setup. Los cambios añaden pushes a todas las ramas, ejecución manual disponible tras publicar el workflow, permisos `contents: read`, checkout sin credenciales persistidas, cancelación de ejecuciones anteriores por referencia y timeout de 15 minutos. Las pruebas unitarias y de integración producen TRX separados; una guarda rechaza cero unitarias ejecutadas sin fijar permanentemente el número 44. Se configuró la subida de TRX con `always()` y retención de 14 días; si el build falla antes de generar informes, la ausencia de archivos produce un aviso y no oculta el fallo anterior.

Las decisiones se contrastaron con fuentes primarias: [compilación y pruebas .NET](https://docs.github.com/en/actions/tutorials/build-and-test-code/net), [sintaxis de workflows](https://docs.github.com/en/actions/reference/workflows-and-actions/workflow-syntax), [concurrencia](https://docs.github.com/en/actions/how-tos/write-workflows/choose-when-workflows-run/control-workflow-concurrency) y [artefactos de resultados](https://docs.github.com/en/actions/tutorials/store-and-share-data). Estas referencias sustentan la configuración; no sustituyen una ejecución remota.

## Verificaciones y resultados reales

Variables de proceso locales: `DOTNET_CLI_HOME=.local/dotnet`, `DOTNET_SKIP_FIRST_TIME_EXPERIENCE=1`, `DOTNET_CLI_TELEMETRY_OPTOUT=1` y `DOTNET_ADD_GLOBAL_TOOLS_TO_PATH=0`. No se alteró configuración de máquina. Los pasos .NET se ejecutaron fuera del sandbox con aprobación por los permisos de los artefactos existentes.

```powershell
dotnet restore ProdeChamp.slnx
dotnet build ProdeChamp.slnx --no-restore --configuration Release
dotnet test tests/ProdeChamp.UnitTests/ProdeChamp.UnitTests.csproj --no-build --no-restore --configuration Release --logger 'trx;LogFileName=unit-tests.trx' --results-directory TestResults/unit
dotnet test tests/ProdeChamp.IntegrationTests/ProdeChamp.IntegrationTests.csproj --no-build --no-restore --configuration Release --logger 'trx;LogFileName=integration-tests.trx' --results-directory TestResults/integration
```

| Comprobación | Estado | Resultado observado |
| --- | --- | --- |
| Restauración | Verificado localmente | Seis proyectos restaurados, exit 0. |
| Compilación Release | Verificado localmente | Seis proyectos, exit 0, 0 advertencias y 0 errores. |
| Pruebas unitarias Release | Verificado localmente | Exit 0; 44 ejecutadas/aprobadas, 0 fallidas y 0 omitidas. |
| Guarda de ejecución unitaria | Verificado localmente | El bloque exacto extraído del workflow aceptó el TRX real con 44 ejecutadas. |
| Guarda ante cero pruebas | Verificado localmente | El mismo bloque rechazó un TRX sintético con `executed=0`, con la excepción esperada. Fixture aislado en un directorio temporal; no se sobrescribió evidencia real. |
| Sintaxis PowerShell de la guarda | Verificado localmente | Parser de PowerShell sin errores. |
| Pruebas de integración Release | Cero pruebas descubiertas | Exit 0, total/ejecutadas/aprobadas = 0; no establece cobertura de integración. |
| Skill | Verificada estructuralmente | Frontmatter `name`/`description`, nombre coincidente con carpeta, límites de formato, ausencia de placeholders y enlaces locales comprobados. Inventario: 11 Skills. |
| Validador Python de skill-creator | No ejecutado | Python no está disponible en PATH; se leyó `quick_validate.py` y se realizaron comprobaciones con PowerShell. No se presenta como validación del script oficial. |
| YAML/GitHub Actions | Inspección manual | No hay actionlint ni parser YAML disponible; no se afirma validación completa del esquema. Los comandos y la guarda se ejecutaron localmente. |
| Subida de artefactos, triggers y concurrencia en GitHub | No ejecutado para esta versión | Requiere publicar los cambios y una ejecución de GitHub Actions. |

Los TRX reales se conservan en `TestResults/unit/unit-tests.trx` y `TestResults/integration/integration-tests.trx`, ignorados por Git. El intento anterior a la aclaración del usuario fue interrumpido y no se contabiliza como una ejecución aprobada; los resultados anteriores corresponden a la aplicación completada de la nueva Skill.

## Estado remoto observado

Las consultas de lectura con `gh` recuperaron la Issue #1 abierta, ningún PR abierto y dos ejecuciones exitosas de `.NET CI` en `main`. La última observada fue [39978279692](https://github.com/DiegoDX/ProdeChampIA/actions/runs/39978279692), commit `d4d85f03edf38af3b07848abbef47ac3dc8f44d4`; la anterior, [39673364514](https://github.com/DiegoDX/ProdeChampIA/actions/runs/39673364514), commit `9ef10be1129c2a41ccfa51c6407a3a605318d361`. Ninguna corresponde al árbol de trabajo de PRODE-001 ni a la nueva configuración.

No se hicieron commits, pushes, cambios de Issues, PRs, dispatches, reruns, cancelaciones ni merges remotos. La lectura inicial en sandbox falló por red; la consulta fuera del sandbox, autorizada, permitió obtener los resultados históricos. No se expusieron credenciales.

## Limitaciones y siguiente etapa

- La Skill existe en `.agents/skills/ci/` y ya se aplicó leyendo ese archivo. La interfaz puede necesitar recargar la sesión para mostrarla en su catálogo de Skills; no se verificó ese catálogo visual.
- Se prepararon los enlaces de solución para inspeccionar Skill, workflow e informe en Visual Studio; la comprobación de rutas es independiente de la actualización visual del IDE.
- El CI remoto de esta versión está pendiente. La subida de TRX no se probó contra el servicio de artefactos de GitHub.
- La verificación local usó Windows PowerShell disponible y .NET 10; el runner de GitHub ejecutará la guarda con `pwsh`. No se afirma equivalencia completa de entornos ni comprobación YAML con un linter.
- La creación de una Skill de CI no reemplaza la revisión independiente de código ni la aprobación humana. La implementación y la publicación siguen sujetas a esas etapas. CD queda fuera del alcance actual.

Para volver a usarla: «Usa la Skill ci de .agents/skills/ci/SKILL.md para verificar el CI de PRODE-001 y reportar por separado los resultados locales y de GitHub, sin publicar cambios».

## Preguntas de entrevista

1. **¿Por qué `dotnet test` con exit 0 no basta para declarar pruebas aprobadas?** Puede descubrir cero pruebas; la guarda comprueba ejecución y el informe distingue el proyecto de integración vacío.
2. **¿Qué diferencia hay entre verificar localmente el workflow y obtener CI verde?** Localmente se prueban comandos y checks; el resultado remoto debe asociarse al commit y run correctos, incluidos runner y acciones.
3. **¿Por qué separar CI de CD en este proyecto?** Build y tests tienen alcance y evidencia actuales; desplegar requiere decisiones de entorno y operación que todavía no están aprobadas.
4. **¿Qué aporta conservar TRX con `always()`?** Facilita diagnosticar fallos después de la ejecución; la evidencia disponible depende de hasta qué paso llegó el job y no debe ocultar un fallo anterior.

## Seguimiento — revisión, publicación y CI remoto, 2026-10-27

La revisión independiente solicitada con la Skill `code-review` comparó `main` (`d4d85f0`) contra la rama y su árbol de trabajo completo: 21 archivos, incluyendo el commit previo de la Skill architect, los cambios sin commit y los archivos sin seguimiento. Consultó la Issue #1, plan, arquitectura, informes y los 14 TRX locales conservados. Dictamen consultivo: **APPROVED**, sin hallazgos P0–P3 accionables. No ejecutó build ni pruebas. Los tres hashes de fuente/pruebas coincidieron con los registrados arriba.

El usuario autorizó expresamente el commit, push y creación del PR, sin fusión. Se creó el commit `fe9155f1f81cbd4654a7c46084cfb45fe1b85ce6` (`feat(PRODE-001): implement prediction scoring and negative-goal validation`), con los 20 archivos locales revisados; la Skill architect ya estaba en el commit anterior de la rama. Se publicó `feature/PRODE-001-scoring-engine` y se creó [PR #3](https://github.com/DiegoDX/ProdeChampIA/pull/3) hacia `main`. La comprobación remota confirmó que `main` seguía en `d4d85f03edf38af3b07848abbef47ac3dc8f44d4`.

### Evidencia remota comprobada

- Run: [40529598727](https://github.com/DiegoDX/ProdeChampIA/actions/runs/40529598727), evento `push`, rama `feature/PRODE-001-scoring-engine`, SHA `fe9155f1f81cbd4654a7c46084cfb45fe1b85ce6`, conclusión `success`.
- Job: [build-and-test](https://github.com/DiegoDX/ProdeChampIA/actions/runs/40529598727/job/122278457561), completado el 2026-10-27 a las 15:19:11 UTC.
- Restore y build Release: pasos aprobados; el log registra `Build succeeded`, 0 advertencias y 0 errores.
- Unitarias: TRX descargado e inspeccionado; 44 resultados individuales, 44 ejecutadas/aprobadas, 0 fallidas y 0 no ejecutadas. Desglose: 34 de puntuación y 10 de validación.
- Guarda de ejecución: paso aprobado contra el TRX real en GitHub. La comprobación negativa sintética continúa siendo evidencia local histórica, no una ejecución remota adicional.
- Integración: paso aprobado con 0 pruebas disponibles; el TRX tiene total/ejecutadas/aprobadas = 0. No acredita cobertura de integración.
- Artefacto [test-results](https://github.com/DiegoDX/ProdeChampIA/actions/runs/40529598727/artifacts/12744819564): subida aprobada, ID `12744819564`, 8958 bytes, ambos TRX descargados en `.local/PRODE-001-github-40529598727/`, fuera del control de versiones.

Consultas realizadas: `gh run list`, `gh run view 40529598727 --json status,conclusion,headSha,url,jobs`, `gh run view 40529598727 --log` y `gh run download 40529598727 --name test-results`. La lectura inicial de GitHub y escritura del índice fallaron por restricciones del sandbox; se completaron con ejecución aprobada. No son fallos de producto ni de pruebas.

### Sincronización documental y límites

Este seguimiento actualiza únicamente el estado y enlaces de README raíz, los README de docs/arquitectura/producto/testing/user-stories, el plan, ambos informes de ingeniería y este informe CI. No cambia calculador, suites, proyectos, solución, workflow ni reglas. Se conserva la evidencia histórica y se distingue la autorización para publicar de la revisión humana final del PR. Los documentos ya están vinculados en la solución.

No se repitieron build ni pruebas localmente durante esta publicación. El run remoto anterior corresponde exactamente a `fe9155f`; el commit posterior de sincronización documental tendrá sus propias comprobaciones de push y PR. Sus resultados se consultarán antes de la entrega y se resumirán en el PR, sin atribuir al run anterior la validación de un SHA distinto. No se midió cobertura, no hay pruebas de integración y no se verificó ejecución manual ni cancelación por concurrencia. No se fusionó el PR ni se modificaron los criterios de la Issue.

Pregunta de entrevista adicional: **¿Por qué registrar el SHA del run y volver a comprobar CI tras un commit documental?** Cada resultado remoto acredita el árbol de su propio commit; un resultado exitoso anterior no sustituye los checks del head final del PR.
