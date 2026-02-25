# ML-Breakout
OSU CS467 Capstone Project - A Unity based Atari Breakout clone with built in human and machine playability for fun and an exercise in machine learning with ML Agents

Brief installation instructions (Windows):
  1) For Unity installation see https://docs.unity3d.com/hub/manual/InstallHub.html
    - Unity Hub https://public-cdn.cloud.unity3d.com/hub/prod/UnityHubSetup-x64.exe
    - Unity [unityhub://6000.3.2f1/a9779f353c9b](https://unity.com/releases/editor/whats-new/6000.3.2f1#notes)
  
  2) For MiniConda https://repo.anaconda.com/miniconda/Miniconda3-latest-Windows-x86_64.exe
  
  3) Open an Anaconda powershell prompt and enter
  
    `conda create -n mlagents python=3.10.12`
  
    `conda activate mlagents`
  
    `conda install numpy=1.23.5`
  
    `pip3 install torch~=2.2.1 --index-url https://download.pytorch.org/whl/cu121`
  
  4) In Unity, open Window > Package Manager.
    - Select + > Add package by name.
    - Enter com.unity.ml-agents. For version, type 4.0.2.
    - Enable Preview Packages under the Advanced drop-down list if the package doesn’t appear.
  
  5) Return to the powershell prompt in step 3 with the mlagents environment activated.
  
    `python -m pip install mlagents==1.1.0`
  
  Additional installation resources can be found at:
  
  - https://docs.unity3d.com/Packages/com.unity.ml-agents@4.0/manual/Installation.html#install-ml-agents
  - https://www.youtube.com/watch?v=bT3SV1SLqHA