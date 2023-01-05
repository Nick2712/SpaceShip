using UnityEngine;
using UnityEngine.Rendering;


namespace SpaceShip
{
    public class SpaceRunPipelineRender : RenderPipeline
    {
        private CameraRenderer _cameraRenderer;


        protected override void Render(ScriptableRenderContext context, Camera[] cameras)
        {
            _cameraRenderer = new CameraRenderer();
            CamerasRender(context, cameras);
        }

        private void CamerasRender(ScriptableRenderContext context, Camera[] cameras)
        {
            foreach (var camera in cameras)
            {
                _cameraRenderer.Render(context, camera);
            }
        }
    }
}
