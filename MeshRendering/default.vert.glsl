#version 330 core

uniform mat4 MVP_Matrix;
uniform mat4 modelMatrix;

layout ( location = 0 ) in vec3 vPos;
layout ( location = 1 ) in vec3 vColor;

out vec3 Color;

void main()
{
	vec4 pos = MVP_Matrix * modelMatrix * vec4(vPos, 1.0);

	gl_Position = pos;
	Color = vColor;
}