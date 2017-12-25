#!/usr/bin/env python
# -*- coding: cp1251 -*-
import requests
import os
ApiKey = 'cc4b67c52acb514bdf4931f7cedfd12b'

def GetPage(id):
    url = 'https://api.themoviedb.org/3/tv/top_rated?api_key=cc4b67c52acb514bdf4931f7cedfd12b&language=ru-RU&page='+str(id)
    return requests.get(url,{'api_key':ApiKey}).json()['results']

def IsPermissibleGenre(tvId):
    
    url = 'https://api.themoviedb.org/3/tv/'+str(tvId)
    genres = requests.get(url,{'api_key':ApiKey}).json()['genres']

    for g in genres:
        if g['name']=='Animation' or g['name']=='Documentary':
            return False
        
    return True

def GetImagesTv(tvId):
    
    url = 'https://api.themoviedb.org/3/tv/%s/images'%(str(tvId))
    return requests.get(url,{'api_key':ApiKey}).json()['backdrops']

def DownloadFile(url,name):
    name = name.strip().replace(':','')
    if not os.path.exists('images'):
        os.makedirs('images\\'+name)
    if not os.path.exists('images\\'+name):
        os.makedirs('images\\'+name)
        
    local_filename = 'images\\'+name+'\\'+url.split('/')[-1]

    r = requests.get(url, stream=True)
    with open(local_filename, 'wb') as f:
        for chunk in r.iter_content(chunk_size=1024): 
            if chunk: # filter out keep-alive new chunks
                f.write(chunk)
    return local_filename

for i in range(1,11):
    #try:
        page =  GetPage(i)
        for tv in page:
            if tv['overview']=='' or IsPermissibleGenre(tv['id']) == False:
                continue
            print(tv['name'])
            images = GetImagesTv(tv['id'])
            for img in images:
                if img['aspect_ratio']>1.7 and img['iso_639_1'] is None:
                    DownloadFile('https://image.tmdb.org/t/p/w1280/'+img['file_path'],tv['name'])
    #except:
     #   print("Косячище"
print ('!!!END!!!')