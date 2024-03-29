using UnityEngine;
using System;
using System.Collections;
using live2d;

namespace Live2App
{
	public class GamePadMain : MonoBehaviour
	{
		public TextAsset mocFile ;
		public Texture2D[] texture ;
		private Live2DModelUnity live2DModel;

		// mtnファイル設定用
		public TextAsset[] mtnFiles;

		// Live2Dモーション用
		private Live2DMotion 		motion;
		private MotionQueueManager 	motionManager;
		private MotionQueueManager 	motionManager2;


		void Start ()
		{
			Live2D.init();

			live2DModel = Live2DModelUnity.loadModel(mocFile.bytes);
			for(int i = 0; i < texture.Length; i++)
			{
				live2DModel.setTexture( i, texture[i] ) ;
			}

			// モーションのロード
			motion = Live2DMotion.loadMotion( mtnFiles[ 0 ].bytes );
			motion.setLoop( true );
			// モーション管理クラスのインスタンスの作成
			motionManager = new MotionQueueManager();
			motionManager2 = new MotionQueueManager();
			// モーションの再生
			motionManager.startMotion( motion, false );
		}

		void Update(){
			// Fire1 : マウス左クリック or 左Ctrlキー or ジョイスティックボタン 0
			if( Input.GetButtonDown( "Fire1" ) ){
				motion = Live2DMotion.loadMotion( mtnFiles[ 1 ].bytes );
				motion.setLoop ( true );
				motionManager.startMotion( motion, false );
				Debug.Log("Fire1");
			}
			if( Input.GetButtonDown( "Fire2" ) ){
				motion = Live2DMotion.loadMotion( mtnFiles[ 2 ].bytes );
				motion.setLoop ( true );
				motionManager.startMotion( motion, false );
				Debug.Log("Fire2");
			}
			if( Input.GetButtonDown( "Fire3" ) ){
				motion = Live2DMotion.loadMotion( mtnFiles[ 0 ].bytes );
				motion.setLoop ( true );
				motionManager.startMotion( motion, false );
				Debug.Log("Fire3");
			}
			if( Input.GetButtonDown( "Jump" ) ){
				motionManager.stopAllMotions();
				Debug.Log("Jump");
			}

		}

		void OnRenderObject()
		{
			Matrix4x4 m1 = Matrix4x4.Ortho(0,live2DModel.getCanvasWidth(),live2DModel.getCanvasWidth(),0,-0.5f,0.5f);
			Matrix4x4 m2 = transform.localToWorldMatrix;
			Matrix4x4 m3 = m2*m1;
			live2DModel.setMatrix(m3);
			if( live2DModel == null ) return ;

			//double t = (UtSystem.getUserTimeMSec()/1000.0) * 2 * Math.PI  ;
			//live2DModel.setParamFloat( "PARAM_ANGLE_X" , (float) (30 * Math.Sin( t/3.0 )) ) ;

			// 再生中のモーションからモデルパラメータを更新
			motionManager.updateParam( live2DModel );
			motionManager2.updateParam( live2DModel );

			live2DModel.update();
			live2DModel.draw();
		}
	}
}