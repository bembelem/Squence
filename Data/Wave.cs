using System.Collections.Generic;

namespace Squence.Data
{
    public readonly record struct Wave(
        List<WavePhase> WavePhasesList,
        float WavePhasesDelay
    );
}
