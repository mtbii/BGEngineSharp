#version 130

out vec4 color;

uniform vec4 diffuseColor;
uniform vec4 ambientColor;
uniform vec4 specularColor;
uniform float hardness;

void main(){
   color = diffuseColor;
}