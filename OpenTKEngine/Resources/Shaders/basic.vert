#version 130

in vec3 vertexPosition;
in vec2 vertexTexCoord;
in vec3 vertexNormal;

out vec2 texCoord0;

uniform mat4 mvpMat;
uniform mat4 mvMat;
uniform mat4 normalMat;

void main(){
   gl_Position.xyzw = mvpMat*vec4(vertexPosition, 1.0);
   texCoord0 = vertexTexCoord;
}