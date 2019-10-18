# UnityChan-NeonStage  
[動画](https://www.youtube.com/watch?v=unhS_zBxCpI)  


### [コンセプト]  

#### ネオンライトを見つめ続けると頭がボーとしてくるようなどこか現実とは浮世離れしたステージをイメージした。  

*** 

### [制作環境]  

MacBookPro 画素：2560 x 1600(RetinaDisplay)  

#### (使用ツール)  
##### □Houdini □unity(Lwrp) □photoshop  
#### (アセット)  
##### 無し  
#### (使用Api)  
##### unitychan-crs-master / Lasp-master  
https://github.com/unity3d-jp/unitychan-crs  
https://github.com/keijiro/Lasp  

*** 

### [参考資料]  
#### pintarest(mylist) / エルサとサメのぽきMV / キズナアイMV /輝夜ルナlive.promotion / Yuni Mv + live  

*** 

### [エラー一覧]  
#### そうこなくっちゃ面白く無い。潰した・潰し損ねた主なエラー一覧  

#### □unitychan-crs-masterからのunityちゃんの移植   
SRPからLwrpに何か持って来る際に必ず起きるシェーダーのエラーはお約束なので対策もバッチリ！  
RenderPipelineからupgradeをすれば一括で治るはず。。。。治らない。。めっちゃピンクである。。。  
おそらくunityちゃんのshaderはレガシーshaderだからなのかな？(適当)  
結局shaderを作り直した。　ついでにテクスチャーの色相変換をして少しオリジナルの衣装とフリルを電飾にしてディゾルブモーションをTimelineでstartとendに組み込んだ  
けど、本家の質感(ヌメヌメした質感)をどうしても再現できない。。。どうやってんの。。  

#### □ Postprossingのエラーに関して  
unityUpdateの度に起きてるLwrpでのPostprossingのエラー。  
stackとかが出てきたあたりからガイドラインがあやふやに感じる(クレーム)  
postprocess LayerをAdd CompornentからではなくWindowから追加したら何故かエラーが消えた。 
~~windows用にbuildした際にコンソールにスクリプトエラーが出るがBuildは成功するし、~~ (宣言のあやふやの警告がでる、対処済み)挙動に問題は無い。 
意味がわからない。  

#### □RenderTextureの写り込みに自身のオブジェクトが写ってしまう問題。  
RenderTextureによりライブスクリーンを作り出す際、自身のオブジェクトが写ってしまう。  
これはそもそもRenderTextureは指定したカメラのシーンをそのまま映し出す、要は鏡のような役割をするものなので当たり前の挙動。  
解決としてはLayerを指定し、使用するカメラからそのレイヤーを除外すれば写り込みはしなくなる。  


#### □Bloomでの白飛びに関して  
完全に白飛びしてしまうケース。  
対策は簡単でbloomをかけ過ぎ、あるいは環境光が白すぎるのが原因なのでpostprossingあるいはLightingの設定を適当にすれば良いのだが  
今回はbloom特有のちょっとかけ過ぎた際の滲むような感じを利用したかった為、調整に少し手間取った。  
DepthOfFieldをちょっと強めにしてあげる事で良い感じに。  
あとはTextureのshaderのemmisionの値を調整してあげるとグッド。  

#### □床面反射ができない件  
今回唯一挫折した床の反射。  
まずはRefrection Proveでの実装を試したが映らない、MetalicMapの強い物体への反射しか効かず?  
写したい床は透明なので話にならない。  
次にRenderTextureで床下からのカメラ情報を組み込む方法を試したが、、、写り込みはするけど汚っいしなんか上手くブレンドしない、なんか白い  
アイデアは良かったけど妥協できないレベルだったのでボツ。  
最終手段で負荷は高いがPostProssingのScreenSpaceRefrectionの実装にするかぁ(unityでの反射は恐らくほぼこれを使ってるってくらい優秀な反射ができる)  
と思ってましたが、、、Lwrpではまだ開発途中で使用できないそうです、、、じぇじぇじぇ  
思いつきでシェーダーを2パス書いたらいけるんじゃ無いかと思いやってみたが、汚い上に、unityちゃんが右に行ったら反射は左に歩いてしまうと言う怪奇現象が起こるのでボツ。  
力が欲しい。  

<img width="500" alt="mira_test" src="https://user-images.githubusercontent.com/43961147/66978502-2a568100-f0e5-11e9-80d7-f24b798ce447.png">  

*** 

### [ステージ紹介]   

#### □ライブスクリーン□  
<img width="600" alt="u_4" src="https://user-images.githubusercontent.com/43961147/66817698-168d0c80-ef77-11e9-8b9a-f281cbf25d7f.png">  

RenderTextureにシェーダーでuvのyに対してsinカーブをめっちゃ入れる、するとシマシマ模様ができるのでこれをtimeで動かすと俗に言うテレビクラッチ効果になる  
そこに少数以下でtimeノードを反復させる、さらにランダムにさせるとチカチカできる。  
最終的には六角形にAlfa値を使って切り抜く。  

#### □ムービングライト□  
<img width="600" alt="u_5" src="https://user-images.githubusercontent.com/43961147/66816980-f3ae2880-ef75-11e9-8d6e-8c0b2fe66e17.png">  

恐らく令和で流行る仮想世界でのムービングライト  
まず、houdiniで円錐型のオブジェクトを作りobjで出力(無料版はfbx無理なんだな)  
シェーダーでカメラ深度とフレネル(法線とカメラベクトルの差分(ベクトルの内積))をぶちこむ。  
スクリプトで特定のターゲット(gameobject)に対して向きを追従するようにする。  
cinemachineのドリーターゲットでgameobjectを動かす。  
玉の部分はuvスクロールにフレネルで中心を光らせた物  
いい感じに調整する。 

#### □ハニカム型の動く床□  

<img width="600" alt="u_3" src="https://user-images.githubusercontent.com/43961147/66816981-f446bf00-ef75-11e9-8c23-31858444fe5f.png">  

uvで六角形を作り距離関数で動かし、Alfaclipで黒い部分を切り抜く  
[詳細](https://github.com/naoto-leon/Hanikam_shader)  

#### □音に反応する光る電飾板□   

<img width="600" alt="u_2" src="https://user-images.githubusercontent.com/43961147/66816982-f446bf00-ef75-11e9-8d08-d1a8c3b61c75.png">  

fractionで切り分けしたuvmapにpostrize(詳細はわからないが値に合わせて恐らく色相分解してくれるのかな)でアレンジを加えたものに  
スクリプトでノイズの値を音に合わせて変化させる。  
両面描画にしてquadではなく薄くしたboxにする事で奥行き感を演出できる(二面描画ぽくなる)  
[詳細](https://github.com/naoto-leon/NoizeHole)  

#### □音に反応するスピーカー□   

<img width="600" alt="u_1" src="https://user-images.githubusercontent.com/43961147/66816983-f446bf00-ef75-11e9-8c97-dd17d438129d.png">  

今回はparticleで実装(良い感じのtextureを持っていた為)  
particleはネオン系のエフェクトにメチャクチャ強い気がする(ゲームエフェクトは基本光るからかな)  
particleのstartsizeの値をスクリプトで音に合わせて動的に変化させる。  

#### □音に合わせて変形して光る壁□  

<img width="600" alt="u_6" src="https://user-images.githubusercontent.com/43961147/66816984-f446bf00-ef75-11e9-8c23-921bc0f31782.png">  

ラインレンダラーとキーフレームを利用しての音に合わせての動的変化  
[詳細](https://github.com/naoto-leon/Box-Chain)  

*** 

### [Statistics  (動作状況)]

<img width="600" alt="u_7" src="https://user-images.githubusercontent.com/43961147/66824813-ffa0e700-ef83-11e9-8917-ecb4353f1c98.png">  

fpsを軽くする作業はいつも同じことしかしないのですが、他企業のCEOとかに聞いても同じような感じなので恐らくそんな感じなのでしょう(笑)。  
唯今回はstaticバッチングもしなかったし、マテリアルを一つにまとめることもしなかった。  
メッシュの結合(バッチング)(Mesh Bakerを使うのが主流ぽい)に関してもそもそも3Dモデルが少ないので行わなかった。  
案の定Batchesが上がり、SetPassCallが100を超えてしまった(笑)  
pcであればBatchesもvertsも1000くらいは余裕なので良いけど、恐らくモバイルでは動かないのでそれ用に対応が必要。  
恐らく透明シェーダーの負荷が重い気がする。  
GpuインスタンスやparticleのGpu使用、postprosessの高速化でfpsは100以上あるので90fpsが最低ラインのVRでは可能性がなくは無い。  

***

### [あとがき]  

ステージ作りはめちゃくちゃ楽しい。  
何よりも世界観やコンセプトを考えるのがVJとは違って物語性があって深みがある。  
今回timelineを初めて結構どっぷり使った、技術的な事では問題が無い(調べれば大抵出てくる)がカメラワークには結構苦労させられた。  
この何回も見直して調整していく作業をどうにか簡略化できないか考える必要がある。  
また、VチューバーのMVを結構参考にしたが,エモいカメラワークやライブ演出の経験値が足りて無いと感じた。  
timelineが出た際は正直バカにしていたが（こんなツール重くて使い物にならないと思っていた）使ってみて非同期処理の手軽さに癖になりそうな快適さがあった。  
世間的にもUnityのTimelineの優秀さが浸透しているように感じる。  






