#version 130

in vec3 vertexPosition;

uniform mat4 mvpMat;

void main(){
	gl_Position.xyzw = mvpMat*vec4(vertexPosition, 1);
}