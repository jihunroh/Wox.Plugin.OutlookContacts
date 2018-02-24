#!/bin/bash

# only proceed script when started not by pull request (PR)
if [ $TRAVIS_PULL_REQUEST == "true" ]; then
  echo "this is PR, exiting"
  exit 0
fi

# enable error reporting to the console
set -e

git clone -b jekyll-src https://${GH_TOKEN}@github.com/jihunroh/jihunroh.github.io.git ../jekyll-src
cp -R ../jekyll-src/_layouts/ ../jekyll-src/_includes/ ../jekyll-src/_config.yml docs/
cd docs
bundle exec jekyll build --profile

# cleanup
rm -rf ../jihunroh.github.io.master
#clone `master' branch of the repository using encrypted GH_TOKEN for authentification
git clone -b master https://${GH_TOKEN}@github.com/jihunroh/jihunroh.github.io.git ../jihunroh.github.io.master
# cleanup repository
rm -rf ../jihunroh.github.io.master/projects/Wox.Plugin.OutlookContacts/
mkdir ../jihunroh.github.io.master/projects/Wox.Plugin.OutlookContacts/
# copy generated HTML site to `master' branch
cp -R _site/* ../jihunroh.github.io.master/projects/Wox.Plugin.OutlookContacts

# commit and push generated content to `master' branch
# since repository was cloned in write mode with token auth - we can push there
cd ../jihunroh.github.io.master
git config user.email "jihunroh@outlook.kr"
git config user.name "JIHUN ROH"
git add -A .
git commit -a -m "Wox.Plugin.OutlookContacts: Travis #$TRAVIS_BUILD_NUMBER"
git push origin master