using UnityEngine.Rendering.Universal;

namespace GameOptions
{
    // Opciones propias en lugar de valores crudos de URP: el jugador elige "Medias",
    // y la traduccion a numeros concretos vive en un unico lugar (GraphicsOptionsExtensions).
    // Si se cambia el orden de un enum, hay que actualizar las etiquetas en GraphicsSection.

    public enum ShadowSetting
    {
        Off,
        Low,
        Medium,
        High
    }

    public enum AntialiasingOption
    {
        Off,
        FXAA,     // Post-proceso, muy barato, algo borroso
        SMAA,     // Post-proceso, más nitido que FXAA
        MSAA2x,   // Multisampling (hardware), suaviza bordes de geometria
        MSAA4x,
        MSAA8x
    }

    public static class GraphicsOptionsExtensions
    {
        // Sombras: distancia maxima (metros) y resolucion del shadow map
        // Valores de referencia: cada proyecto deberia ajustarlos a su escala y rendimiento.

        public static float ToShadowDistance(this ShadowSetting setting) => setting switch
        {
            ShadowSetting.Low => 25f,
            ShadowSetting.Medium => 50f,
            ShadowSetting.High => 100f,
            _ => 0f // Distancia 0 = URP no dibuja sombras
        };

        public static int ToShadowmapResolution(this ShadowSetting setting) => setting switch
        {
            ShadowSetting.Low => 512,
            ShadowSetting.Medium => 1024,
            ShadowSetting.High => 2048,
            _ => 256
        };

        // Antialiasing: en URP se reparte en dos lugares
        // MSAA se configura en el URP Asset; FXAA/SMAA en cada camara.

        public static int ToMsaaSampleCount(this AntialiasingOption option) => option switch
        {
            AntialiasingOption.MSAA2x => 2,
            AntialiasingOption.MSAA4x => 4,
            AntialiasingOption.MSAA8x => 8,
            _ => 1 // 1 muestra = MSAA desactivado
        };

        public static AntialiasingMode ToCameraAntialiasing(this AntialiasingOption option) => option switch
        {
            AntialiasingOption.FXAA => AntialiasingMode.FastApproximateAntialiasing,
            AntialiasingOption.SMAA => AntialiasingMode.SubpixelMorphologicalAntiAliasing,
            _ => AntialiasingMode.None
        };
    }
}
