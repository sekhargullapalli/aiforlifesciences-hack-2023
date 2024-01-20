<img src="https://github.com/sekhargullapalli/aiforlifesciences-hack-2023/actions/workflows/main_ai4ls.yml/badge.svg" alt="AI4LS Deployment">


#### Final Submitted Solution (Web App)

[<img src="./images/ai4ls.png" width=125>](https://ai4ls.azurewebsites.net)
<br/>
<br/>

The **final solution** is a web application hosted on Azure App Service and is published using github actions. Please click the link above to access the web application. The following information is available in the web app.

1. About Team
2. Repository
3. Final Report
4. Some prediciton UI and notebook viewer
5. OpenAI based bot
6. Visualzation layer for LUCAS Data

<br/>

# [Solution Notion Site / Report](https://clammy-newt-56c.notion.site/EcoSynth-Understanding-and-Enhancing-Soil-Health-and-Microbial-Biodiversity-Using-Artificial-Intel-c8b8b9d149fe4b33bce7fea23570a1e3)
<br/>

<img src="./images/important.png" width=50>

**Please look in the /src folder for Notebooks, WebApp source, Utilities etc.,**
<br/>

# EcoSynth AI
## January 21, 2024

### Datasets
The land usage data for years 2009, 2015 and 2018 is available in this repository.
The microbiome sequence data is available for a short while [here](https://vaquitatechnologies-my.sharepoint.com/:f:/g/personal/vijayasekhar_gullapalli_vaqtech_com/Ep0R0pypvWlOn1HB9-NDXY0BagmWwyPKlZrB2dQ-Efv_5w?e=o9O3Jw). The total size of microbiome sequence data is around 75 GB.

Here is the overlay of land usage data.

<img src="./images/LandUsageDataOverlay.png" alt="LandUsageDataOverlay.png" width=300>


### Tools & Applications
#### GIS (Geographical Information Systems)
[QGIS](https://qgis.org/en/site/) is a free open source geographical information system. Using this we can visualize the ESRI data in LUCAS datasets. Documentation [link](https://docs.qgis.org/3.4/en/docs/index.html)

[ARCGIS Maps SDK for .NET](https://developers.arcgis.com/net/) could be useful, if WPF or Blazor applicaitons are built, for visualization needs.

#### Data Analysis & User Interfaces
For python 3 notebooks, a single virtual environment is used and analysis spread across multiple notebooks. Using Jupyter extension in VSCode.

(.NET 8.0)[https://dotnet.microsoft.com/en-us/download/dotnet/8.0] used for data cleanup, api integrations and some application development. Requires installation of SDK for development. 

After testing WPF, Vanilla JavaScript and Blazor Server, Blazor Server is used to develop the interface due to its flexibility, JS interop and security.

<img src="./images/basemap_js.png" alt="basemap" width="300"/><img src="./images/heatmap_js.png" alt="heatmap" height="377"/>



## License
 
The MIT License (MIT)

Copyright (c) 2015 Chris Kibble

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
